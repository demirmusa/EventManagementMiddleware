using EventManager.Core.Extensions;
using EventManager.DefaultManager.Dto;
using EventManager.DefaultManager.Interfaces;
using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace EventManager.DefaultManager.Extensions
{
    public static class EMDefaultManagerDIExtensions
    {
        /// <summary>
        /// Adds default cache manager (which use Memory cache. That's why you have to add memory cache to your project.), default rabbitmq publisher.
        /// <para>If you dont want to use them create your own one and inject them</para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="eventManagerOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddEMDefaultManager(this IServiceCollection services,
             Action<EventManagerOptions> eventManagerOptions = null)
        {
            services.AddSingleton<ICacheManager, DefaultCacheManager>();
            services.AddSingleton<IDefaultRabbitMQPublisher, DefaultRabbitMQPublisher>();

            services.AddEventManagerCore<IDefaultRabbitMQPublisher, ICacheManager>(eventManagerOptions);
            return services;
        }

        public static IServiceProvider InitializeEMDefaultManager(this IServiceProvider provider,
            string hostName = "localhost")
        {
            var _rabbitMQPublisher = provider.GetService<IDefaultRabbitMQPublisher>();

            _rabbitMQPublisher.InitConnectionFactory(new ConnectionFactory() { HostName = hostName });
            _rabbitMQPublisher.InitQueueInfo(new RabbitMQQueueInfoDto
            {
                QueueName = "nodered",
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                Arguments = null
            });
            return provider;
        }
        public static IServiceProvider InitializeEMDefaultManager(this IServiceProvider provider,
            Action<ConnectionFactory> connectionFactoryAction)
        {
            var _rabbitMQPublisher = provider.GetService<IDefaultRabbitMQPublisher>();

            var conFactory = new ConnectionFactory() { HostName = "localhost" };
            connectionFactoryAction.Invoke(conFactory);

            _rabbitMQPublisher.InitConnectionFactory(conFactory);
            _rabbitMQPublisher.InitQueueInfo(new RabbitMQQueueInfoDto
            {
                QueueName = "nodered",
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                Arguments = null
            });
            return provider;
        }
        public static IServiceProvider InitializeEMDefaultManager(this IServiceProvider provider,
            Action<ConnectionFactory> connectionFactory,
            Action<RabbitMQQueueInfoDto> queueInfoAction)
        {
            var _rabbitMQPublisher = provider.GetService<IDefaultRabbitMQPublisher>();

            var conFactory = new ConnectionFactory() { HostName = "localhost" };
            connectionFactory.Invoke(conFactory);

            var queeuInfo = new RabbitMQQueueInfoDto
            {
                QueueName = "nodered",
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                Arguments = null
            };
            queueInfoAction.Invoke(queeuInfo);

            _rabbitMQPublisher.InitConnectionFactory(conFactory);
            _rabbitMQPublisher.InitQueueInfo(queeuInfo);
            return provider;
        }
    }
}
