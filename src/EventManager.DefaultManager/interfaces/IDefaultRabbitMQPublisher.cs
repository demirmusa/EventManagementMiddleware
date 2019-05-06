using EventManager.Core.Interfaces;
using EventManager.DefaultManager.Dto;
using RabbitMQ.Client;

namespace EventManager.DefaultManager.Interfaces
{
    public interface IDefaultRabbitMQPublisher : IMessagePublisher
    {
        void InitConnectionFactory(ConnectionFactory cf);
        void InitQueueInfo(RabbitMQQueueInfoDto queueInfoDto);
    }
}
