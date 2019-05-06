using EventManager.Core.Interfaces;
using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace EventManager.Core
{
    public class EventManagerCore : IEventManagerCore
    {
        EventManagerOptions _options;
        public EventManagerCore(IOptions<EventManagerOptions> options)
        {
            if (options.Value == null)
                throw new Exception("EventManagerOptions can not be null");

            _options = options.Value;
        }

        private string GetEventName(Type t)
        {
            var attributes = t.GetCustomAttributes();
            foreach (var attr in attributes)
            {
                //if class contains EMEventInfoAttribute take event name from attribute
                if (attr is EMEventInfoAttribute eventInfoAttribute)
                    return eventInfoAttribute.EventName;
            }
            return t.Name;//otherwise event name is class name
        }

        #region Async Methods
        public virtual async Task<EMEvent<T>> GetEventAsync<T>(T e)
            where T : IEMEvent
        {
            string eventName = GetEventName(e.GetType());

            var newEvent = new EMEvent<T>()
            {
                EventData = e,
                EventName = eventName
            };


            return newEvent;
        }


        #endregion

        #region Sync Methods
        public virtual EMEvent<T> GetEvent<T>(T e)
          where T : IEMEvent
        {
            string eventName = GetEventName(e.GetType());

            var newEvent = new EMEvent<T>()
            {
                EventData = e,
                EventName = eventName
            };            

            return newEvent;
        }

        #endregion

    }
}
