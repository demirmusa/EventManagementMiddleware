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
        public string EventApiServerAddress { get; set; }
        /// <summary>
        ///false = Close checking whether event is unique by parameters <para>true = Check event on server (whether it is unique and its properties are same with registered one) </para>
        /// </summary>
        public bool CheckEventOnServer { get; set; } = true;
        public int CacheExpireTimeMinute { get; set; } = 30;
    }
}
