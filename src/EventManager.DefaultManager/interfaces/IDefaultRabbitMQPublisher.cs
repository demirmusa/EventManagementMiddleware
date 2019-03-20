using EventManager.Core.interfaces;
using EventManager.DefaultManager.Dto;
using RabbitMQ.Client;

namespace EventManager.DefaultManager.interfaces
{
    public interface IDefaultRabbitMQPublisher : IMessagePublisher
    {
        void InitConnectionFactory(ConnectionFactory cf);
        void InitQueueInfo(RabbitMQQueueInfoDto queueInfoDto);
    }
}
