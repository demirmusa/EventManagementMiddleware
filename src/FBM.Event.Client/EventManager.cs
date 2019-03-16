using FBM.Event.Client.Dto;
using FBM.Event.Client.interfaces;
using FBM.Event.Shared.Dto;
using FBM.Event.Shared.interfaces;
using FBM.Event.UniqueController.Data.dbEntities;
using FBM.Event.UniqueController.Exceptions;
using FBM.Event.UniqueController.interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            if (options.Value.CheckEventOnServer && string.IsNullOrEmpty(options.Value.EventApiServerAddress))
                throw new Exception("EventApiServerAddress can not be null");

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
        public FBMEvent<T> GetEvent<T>(T e)
            where T : IFBMEvent
        {
            string eventName = GetEventName(e.GetType());

            var newEvent = new FBMEvent<T>()
            {
                EventData = e,
                EventName = eventName
            };
            if (_options.CheckEventOnServer)
                CheckEvent(newEvent, eventName);

            return newEvent;
        }
        private void CheckEvent<T>(FBMEvent<T> fBMEvent, string eventName)
            where T : IFBMEvent
        {
            if (_cacheManager.TryGetValue(_options.RegisteredEventsMemoryCacheKey, out Dictionary<string, FBMEventInfoDto> checkedAndCachedEventsDic))
            {
                if (!checkedAndCachedEventsDic.ContainsKey(eventName))// if dictionary contains event info , it means we already check it and add it to dictionary                  
                    GetEventFromStorage(fBMEvent, checkedAndCachedEventsDic);
                //else
                //{
                //    //do nothing. this event is in checked list 
                //}
            }
            else
                LoadAllRegisteredEvents();
        }


        private async void GetEventFromStorage<T>(FBMEvent<T> fBMEvent, Dictionary<string, FBMEventInfoDto> checkedAndCachedEventsDic = null)
            where T : IFBMEvent
        {
            try
            {
                var check = await _eventChecker.CheckOrAddFBMEventInfo(fBMEvent);
                if (check != null && checkedAndCachedEventsDic != null)
                    checkedAndCachedEventsDic.Add(check.EventName, check);
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
        private async void LoadAllRegisteredEvents()
        {
            try
            {
                var list = await _eventChecker.GetAllRegisteredEvents();

                Dictionary<string, FBMEventInfoDto> dict;
                if (list != null && list.Count > 0)
                    dict = list.ToDictionary(x => x.EventName, y => y);
                else
                    dict = new Dictionary<string, FBMEventInfoDto>();

                _cacheManager.Set(_options.RegisteredEventsMemoryCacheKey, dict, TimeSpan.FromMilliseconds(_options.CacheExpireTimeMinute));
            }
            catch (Exception e)
            {
                throw new Exception("Error while getting all event from storage.See inner exception for more information.", e);
            }
        }


    }
}
