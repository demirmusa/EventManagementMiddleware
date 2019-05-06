using EventManager.DefaultManager.Dto;
using EventManager.DefaultManager.Interfaces;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DefaultManager
{
    /// <summary>
    /// Default rabbitmq publisher. Puslish message to rabbitmq queue which your node-red connected.
    /// </summary>
    public class DefaultRabbitMQPublisher : IDefaultRabbitMQPublisher
    {
        protected ConnectionFactory _rabbitMqConnectionFactory;

        protected RabbitMQQueueInfoDto _queueInfoDto;

        /// <summary>
        /// Connection factory for rabbitmq which your nodered connected
        /// </summary>
        /// <param name="cf"></param>
        public virtual void InitConnectionFactory(ConnectionFactory cf) => _rabbitMqConnectionFactory = cf;

        /// <summary>
        /// RabbitMq queue which nodered connected and used. Init queue info at once.than QueueDeclare function will use these parameters
        /// </summary>
        public virtual void InitQueueInfo(RabbitMQQueueInfoDto queueInfoDto) => _queueInfoDto = queueInfoDto;

        /// <summary>
        /// define (RabbitMQ.Client.IModel)channel.QueueDeclare()
        ///<para>publish method call this function to declare queue. Override it to change declaration easily</para>
        /// </summary>
        protected virtual void QueueDeclare(IModel channel)
        {
            channel.QueueDeclare(queue: _queueInfoDto.QueueName,
                                durable: _queueInfoDto.Durable,
                                exclusive: _queueInfoDto.Exclusive,
                                autoDelete: _queueInfoDto.AutoDelete,
                                arguments: _queueInfoDto.Arguments);
        }

        /// <summary>
        /// define  (RabbitMQ.Client.IModel)channel.BasicPublish() to change publish method
        /// <para>publish method call this func to publish it to client </para>
        /// </summary>
        protected virtual void BasicPublish(IModel channel, byte[] msgBytes)
        {
            channel.BasicPublish(exchange: "",
                                routingKey: _queueInfoDto.QueueName,
                                basicProperties: null,
                                body: msgBytes);
        }


        public virtual void Publish(string message)
        {
            if (_rabbitMqConnectionFactory == null)
                throw new Exception("RabbitMQConnection is not initted. Call InitConnectionFactory before publishing");
            if (_queueInfoDto == null)
                throw new Exception("Queue is not initted. Call InitQueueInfo before publishing");

            using (var connection = _rabbitMqConnectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                try
                {
                    QueueDeclare(channel);
                }
                catch (Exception e)
                {
                    throw new Exception("Error when declare queue. For more information see inner exception", e);
                }

                try
                {
                    var msgBytes = Encoding.UTF8.GetBytes(message ?? "");

                    BasicPublish(channel, msgBytes);
                }
                catch (Exception e)
                {
                    throw new Exception("Error while publishing message. For more information see inner exception", e);
                }

            }

        }

        public async Task PublishAsync(string message)
        {
            //create async rabbitmq publisher
            Publish(message);
        }
    }
}
