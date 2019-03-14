using FBM.Event.Client.Dto;
using FBM.Event.Client.Extensions;
using FBM.Event.Client.interfaces;
using FBM.Event.DefaultManager.Dto;
using FBM.Event.DefaultManager.interfaces;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace FBM.Event.DefaultManager.Extensions
{
    public static class FBMEventDefaultManagerDIExtensions
    {    
        /// <summary>
        /// Adds default cache manager (which use Memory cache. That's why you have to add memory cache to your project.), default rabbitmq publisher.
        /// <para>If you dont want to use them create your own one and inject them</para>
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="eventManagerOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddFBMEventClientWithDefaultManager(this IServiceCollection collection, Action<EventManagerOptions> eventManagerOptions)
        {
            collection.AddSingleton<ICacheManager, DefaultCacheManager>();
            collection.AddSingleton<IDefaultRabbitMQPublisher, DefaultRabbitMQPublisher>();

            collection.AddFBMEventClient<IDefaultRabbitMQPublisher, ICacheManager>(eventManagerOptions);
            return collection;
        }

        public static IServiceProvider InitializeFBMEventClientDefaultManager(this IServiceProvider provider, string hostName = "localhost")
        {
            var _rabbitMQPublisher = provider.GetService<IDefaultRabbitMQPublisher>();

            _rabbitMQPublisher.InitConnectionFactory(new ConnectionFactory() { HostName = hostName });
            _rabbitMQPublisher.InitQueueInfo(new QueueInfoDto
            {
                QueueName = "nodered",
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                Arguments = null
            });
            return provider;
        }
        public static IServiceProvider InitializeDefaultRabbitMQPublisher(this IServiceProvider provider,
            Action<ConnectionFactory> connectionFactoryAction)
        {
            var _rabbitMQPublisher = provider.GetService<IDefaultRabbitMQPublisher>();

            var conFactory = new ConnectionFactory() { HostName = "localhost" };
            connectionFactoryAction.Invoke(conFactory);

            _rabbitMQPublisher.InitConnectionFactory(conFactory);
            _rabbitMQPublisher.InitQueueInfo(new QueueInfoDto
            {
                QueueName = "nodered",
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                Arguments = null
            });
            return provider;
        }
        public static IServiceProvider InitializeDefaultRabbitMQPublisher(this IServiceProvider provider, 
            Action<ConnectionFactory> connectionFactory,
            Action<QueueInfoDto> queueInfoAction)
        {
            var _rabbitMQPublisher = provider.GetService<IDefaultRabbitMQPublisher>();

            var conFactory = new ConnectionFactory() { HostName = "localhost" };
            connectionFactory.Invoke(conFactory);

            var queeuInfo = new QueueInfoDto
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
