using EventManager.Core.Dto;
using EventManager.Core.interfaces;
using EventManager.EventChecker.Exceptions;
using EventManager.EventChecker.interfaces;
using EventManager.Shared.Dto;
using EventManager.Shared.interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EventManager.Core
{
    public class EventManagerCore<TCacheManager> : IEventManagerCore
        where TCacheManager : ICacheManager
    {
        TCacheManager _cacheManager;
        EventManagerOptions _options;
        IEventChecker _eventChecker;
        public EventManagerCore(TCacheManager cacheManager, IOptions<EventManagerOptions> options, IEventChecker eventChecker)
        {
            _cacheManager = cacheManager;
            if (options.Value == null)
                throw new Exception("EventManagerOptions can not be null");

            if (string.IsNullOrEmpty(options.Value.RegisteredEventsMemoryCacheKey))
                options.Value.RegisteredEventsMemoryCacheKey = "EventManager.RegisteredEvents";

            _options = options.Value;
            _eventChecker = eventChecker;
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
        public async Task<EMEvent<T>> GetEventAsync<T>(T e)
            where T : IEMEvent
        {
            string eventName = GetEventName(e.GetType());

            var newEvent = new EMEvent<T>()
            {
                EventData = e,
                EventName = eventName
            };
            if (_options.CheckIsEventUnique)
                await CheckEvent(newEvent, eventName);

            return newEvent;
        }

        private async Task CheckEvent<T>(EMEvent<T> fBMEvent, string eventName)
            where T : IEMEvent
        {
            var propJson = _eventChecker.GeneretePropertiesJson(fBMEvent.EventData);
            var dict = GetAllEventsDic();

            if (dict.ContainsKey(eventName))
            {
                if (dict[eventName].EventPropertiesJson == propJson)
                    return;// its ok do nothing                
                else
                    throw new Exception($"Registered event has different properties.Event Name: {eventName}, Registered Prop:{dict[eventName].EventPropertiesJson}, Sended Props: {propJson}");
            }
            else
            {
                await GetEventFromStorage(fBMEvent, dict);
                if (dict[eventName].EventPropertiesJson == propJson)
                    return;// its ok do nothing                
                else
                    throw new Exception($"Registered event has different properties.Event Name: {eventName}, Registered Prop:{dict[eventName].EventPropertiesJson}, Sended Props: {propJson}");

            }
        }
        private Dictionary<string, EMEventInfoDto> GetAllEventsDic()
        {
            if (_cacheManager.TryGetValue(_options.RegisteredEventsMemoryCacheKey, out Dictionary<string, EMEventInfoDto> eventsDic))
                return eventsDic;
            else
                return LoadAllRegisteredEvents();

        }

        private async Task GetEventFromStorage<T>(EMEvent<T> fBMEvent, Dictionary<string, EMEventInfoDto> checkedAndCachedEventsDic = null)
            where T : IEMEvent
        {
            try
            {
                var check = await _eventChecker.CheckOrAddEMEventInfo(fBMEvent);
                if (check != null && checkedAndCachedEventsDic != null)
                {
                    checkedAndCachedEventsDic.Add(check.EventName, check);
                    _cacheManager.Set(_options.RegisteredEventsMemoryCacheKey, checkedAndCachedEventsDic,
                        TimeSpan.FromMilliseconds(_options.CacheExpireTimeMinute));
                }
            }
            catch (EMEventInvalidPropertyException e)
            {
                // event property is wrong. don't let event publishing.
                throw new Exception("Error while checking event property on storage. Your event properties are wrong.See inner exception for more information.", e);
            }
            catch (Exception e)
            {
                throw new Exception("Error while checkng event propert on storage.See inner exception for more information.", e);
            }
        }
        private Dictionary<string, EMEventInfoDto> LoadAllRegisteredEvents()
        {
            try
            {
                var list = _eventChecker.GetAllRegisteredEvents();

                Dictionary<string, EMEventInfoDto> dict;
                if (list != null && list.Count > 0)
                    dict = list.ToDictionary(x => x.EventName, y => y);
                else
                    dict = new Dictionary<string, EMEventInfoDto>();

                _cacheManager.Set(_options.RegisteredEventsMemoryCacheKey, dict, TimeSpan.FromMilliseconds(_options.CacheExpireTimeMinute));
                return dict;
            }
            catch (Exception e)
            {
                throw new Exception("Error while getting all event from storage.See inner exception for more information.", e);
            }
        }


    }
}
