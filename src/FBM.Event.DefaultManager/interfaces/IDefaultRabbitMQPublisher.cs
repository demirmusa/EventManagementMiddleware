using FBM.Event.Client.interfaces;
using FBM.Event.DefaultManager.Dto;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.DefaultManager.interfaces
{
    public interface IDefaultRabbitMQPublisher : IMessagePublisher
    {
        void InitConnectionFactory(ConnectionFactory cf);
        void InitQueueInfo(QueueInfoDto queueInfoDto);
    }
}
