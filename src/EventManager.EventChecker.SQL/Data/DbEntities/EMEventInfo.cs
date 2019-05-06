using System;
namespace EventManager.EventChecker.SQL.Data.DbEntities
{
    public class EMEventInfo
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public string EventPropertiesJson { get; set; }
        public string CreatorClientName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
