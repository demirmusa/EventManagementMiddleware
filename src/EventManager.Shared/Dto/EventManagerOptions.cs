using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Shared.Dto
{
    public class EventManagerOptions
    {
        /// <summary>
        /// You can change it if you already use "EventManager.EventChecker.RegisteredEvents" key in your memory cache
        /// </summary>
        public string RegisteredEventsMemoryCacheKey { get; set; } = "EventManager.EventChecker.RegisteredEvents";
        /// <summary>
        /// Do not allow to send multiple event with same name. All event will be checked by property and name ,dont forget to add checker service
        /// </summary>
        public bool CheckIsEventUnique { get; set; } = false;
        public int CacheExpireTimeMinute { get; set; } = 30;
    }
}
