using EventManager.Core.Dto;
using EventManager.Core.Extensions;
using EventManager.Core.interfaces;
using EventManager.DefaultManager.Dto;
using EventManager.DefaultManager.interfaces;
using EventManager.EventChecker.Dto;
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
        /// <param name="collection"></param>
        /// <param name="eventManagerOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddEMDefaultManager(this IServiceCollection collection,
             Action<EventCheckerOptions> eventCheckerOptionsAction,
             Action<EventManagerOptions> eventManagerOptions = null)
        {
            collection.AddSingleton<ICacheManager, DefaultCacheManager>();
            collection.AddSingleton<IDefaultRabbitMQPublisher, DefaultRabbitMQPublisher>();

            collection.AddEventManagerCore<IDefaultRabbitMQPublisher, ICacheManager>(eventCheckerOptionsAction, eventManagerOptions);
            return collection;
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
