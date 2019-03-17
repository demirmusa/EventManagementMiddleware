using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Client.Dto
{
    public class EventManagerOptions
    {
        /// <summary>
        /// you cant change it if you already use EventManager.RegisteredEvents key in your memory cache
        /// </summary>
        public string RegisteredEventsMemoryCacheKey { get; set; }
        /// <summary>
        /// do not allow to send multiple event with same name, all event will be checked by property and name
        /// </summary>
        public bool CheckIsEventUnique { get; set; } = true;
        public int CacheExpireTimeMinute { get; set; } = 30;
    }
}
