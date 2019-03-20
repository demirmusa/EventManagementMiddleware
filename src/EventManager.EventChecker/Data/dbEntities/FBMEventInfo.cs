using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.UniqueController.Data.dbEntities
{
    public class FBMEventInfo
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public string EventPropertiesJson { get; set; }
        public string CreatorClientName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
