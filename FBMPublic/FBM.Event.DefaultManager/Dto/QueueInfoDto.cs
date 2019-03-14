using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.DefaultManager.Dto
{
    public class QueueInfoDto
    {
        public string QueueName { get; set; } = "nodered";
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
    }
}
