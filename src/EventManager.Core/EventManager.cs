using FBM.Event.Client.Dto;
using FBM.Event.Client.interfaces;
using FBM.Event.Shared.Dto;
using FBM.Event.Shared.interfaces;
using FBM.Event.UniqueController.Exceptions;
using FBM.Event.UniqueController.interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FBM.Event.Client
{
    public class EventManager<TCacheManager> : IEventManager
        where TCacheManager : ICacheManager
    {
        TCacheManager _cacheManager;
        EventManagerOptions _options;
        IEventChecker _eventChecker;
        public EventManager(TCacheManager cacheManager, IOptions<EventManagerOptions> options, IEventChecker eventChecker)
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
                //if class contains FBMEventInfoAttribute take event name from attribute
                if (attr is FBMEventInfoAttribute eventInfoAttribute)
                    return eventInfoAttribute.EventName;
            }
            return t.Name;//otherwise event name is class name
        }
        public async Task<FBMEvent<T>> GetEventAsync<T>(T e)
            where T : IFBMEvent
        {
            string eventName = GetEventName(e.GetType());

            var newEvent = new FBMEvent<T>()
            {
                EventData = e,
                EventName = eventName
            };
            if (_options.CheckIsEventUnique)
                await CheckEvent(newEvent, eventName);

            return newEvent;
        }

        private async Task CheckEvent<T>(FBMEvent<T> fBMEvent, string eventName)
            where T : IFBMEvent
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
        private Dictionary<string, FBMEventInfoDto> GetAllEventsDic()
        {
            if (_cacheManager.TryGetValue(_options.RegisteredEventsMemoryCacheKey, out Dictionary<string, FBMEventInfoDto> eventsDic))
                return eventsDic;
            else
                return LoadAllRegisteredEvents();

        }

        private async Task GetEventFromStorage<T>(FBMEvent<T> fBMEvent, Dictionary<string, FBMEventInfoDto> checkedAndCachedEventsDic = null)
            where T : IFBMEvent
        {
            try
            {
                var check = await _eventChecker.CheckOrAddFBMEventInfo(fBMEvent);
                if (check != null && checkedAndCachedEventsDic != null)
                {
                    checkedAndCachedEventsDic.Add(check.EventName, check);
                    _cacheManager.Set(_options.RegisteredEventsMemoryCacheKey, checkedAndCachedEventsDic,
                        TimeSpan.FromMilliseconds(_options.CacheExpireTimeMinute));
                }
            }
            catch (FBMEventInvalidPropertyException e)
            {
                // event property is wrong. don't let event publishing.
                throw new Exception("Error while checking event property on storage. Your event properties are wrong.See inner exception for more information.", e);
            }
            catch (Exception e)
            {
                throw new Exception("Error while checkng event propert on storage.See inner exception for more information.", e);
            }
        }
        private Dictionary<string, FBMEventInfoDto> LoadAllRegisteredEvents()
        {
            try
            {
                var list = _eventChecker.GetAllRegisteredEvents();

                Dictionary<string, FBMEventInfoDto> dict;
                if (list != null && list.Count > 0)
                    dict = list.ToDictionary(x => x.EventName, y => y);
                else
                    dict = new Dictionary<string, FBMEventInfoDto>();

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
