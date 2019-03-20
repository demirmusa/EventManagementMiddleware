using System.Collections.Generic;
namespace EventManager.DefaultManager.Dto
{
    public class RabbitMQQueueInfoDto
    {
        public string QueueName { get; set; } = "nodered";
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
    }
}
