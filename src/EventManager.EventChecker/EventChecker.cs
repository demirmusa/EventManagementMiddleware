using EventManager.EventChecker.Exceptions;
using EventManager.EventChecker.Interfaces;
using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.EventChecker
{
    public class EventChecker<TCacheManager> : IEventChecker
        where TCacheManager : ICacheManager
    {
        IEventInfoLoader _eventInfoLoader;
        EventManagerOptions _options;
        TCacheManager _cacheManager;
        public EventChecker(IEventInfoLoader eventInfoLoader, TCacheManager cacheManager, IOptions<EventManagerOptions> options)
        {
            _eventInfoLoader = eventInfoLoader;
            _cacheManager = cacheManager;

            if (options.Value == null)
                throw new Exception("EventManagerOptions can not be null");

            _options = options.Value;

        }
        public string GeneretePropertiesJson<T>(T data) where T : IEMEvent
        {
            Dictionary<string, string> propTypeNameDic = new Dictionary<string, string>();
            //get all properties of event 
            var properties = data.GetType().GetProperties();
            foreach (var p in properties)
                propTypeNameDic.Add(p.Name, p.PropertyType.Name);

            return Newtonsoft.Json.JsonConvert.SerializeObject(propTypeNameDic);
        }


        #region sync checker
        public void CheckEvent<T>(EMEvent<T> eMEvent) where T : IEMEvent
        {
            var propJson = GeneretePropertiesJson(eMEvent.EventData);// generate properties json to check its same with registered one
            var dict = GetAllEventsDic();// get registered events

            if (dict.ContainsKey(eMEvent.EventName))//if event exists
            {
                if (dict[eMEvent.EventName].EventPropertiesJson == propJson)//and properties are same
                    return;// its ok do nothing                
                else
                    throw new Exception($"Registered event has different properties.Event Name: {eMEvent.EventName}, Registered Prop:{dict[eMEvent.EventName].EventPropertiesJson}, Sended Props: {propJson}");
            }
            else
            {
                LoadEventInfoFromStorage(eMEvent); // if event dont exist go and query it (we may dont have it in our cache)
                dict = GetAllEventsDic();// get registered events
                if (dict[eMEvent.EventName].EventPropertiesJson == propJson)//after 
                    return;// its ok do nothing                
                else
                    throw new Exception($"Registered event has different properties.Event Name: {eMEvent.EventName}, Registered Prop:{dict[eMEvent.EventName].EventPropertiesJson}, Sended Props: {propJson}");
            }
        }
        private Dictionary<string, EMEventInfoDto> GetAllEventsDic()
        {
            if (_cacheManager.TryGetValue(_options.RegisteredEventsMemoryCacheKey, out Dictionary<string, EMEventInfoDto> eventsDic))
                return eventsDic;
            else
                return LoadAllRegisteredEvents();

        }

        private void LoadEventInfoFromStorage<T>(EMEvent<T> eMEvent)
          where T : IEMEvent
        {
            try
            {
                string eventPropJson = GeneretePropertiesJson(eMEvent.EventData);
                var checkResult = _eventInfoLoader.GetOrAddEMEventInfo(eMEvent, eventPropJson);
                if (checkResult != null)
                {
                    if (checkResult.EventPropertiesJson != eventPropJson)//if event has different properties .Throw Exception
                        throw new EMEventInvalidPropertyException($"{eMEvent.EventName} named event has different properties. Registered properties: {checkResult.EventPropertiesJson}, Sended properties {eventPropJson}");

                    var dict = GetAllEventsDic();
                    if (dict == null)
                        dict = new Dictionary<string, EMEventInfoDto>();

                    dict.Add(checkResult.EventName, checkResult);

                    _cacheManager.Set(_options.RegisteredEventsMemoryCacheKey, dict, TimeSpan.FromMilliseconds(_options.CacheExpireTimeMinute));
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
                var list = _eventInfoLoader.GetAllRegisteredEvents();

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
        #endregion sync checker







        #region Async Checker
        //------------------ same functions but async 

        public async Task CheckEventAsync<T>(EMEvent<T> eMEvent) where T : IEMEvent
        {
            var propJson = GeneretePropertiesJson(eMEvent.EventData);
            var dict = await GetAllEventsDicAsync();

            if (dict.ContainsKey(eMEvent.EventName))
            {
                if (dict[eMEvent.EventName].EventPropertiesJson == propJson)
                    return;// its ok do nothing                
                else
                    throw new Exception($"Registered event has different properties.Event Name: {eMEvent.EventName}, Registered Prop:{dict[eMEvent.EventName].EventPropertiesJson}, Sended Props: {propJson}");
            }
            else
            {
                await LoadEventInfoFromStorageAsync(eMEvent);
                dict = await GetAllEventsDicAsync();
                if (dict[eMEvent.EventName].EventPropertiesJson == propJson)
                    return;// its ok do nothing                
                else
                    throw new Exception($"Registered event has different properties.Event Name: {eMEvent.EventName}, Registered Prop:{dict[eMEvent.EventName].EventPropertiesJson}, Sended Props: {propJson}");

            }
        }

        private async Task LoadEventInfoFromStorageAsync<T>(EMEvent<T> eMEvent)
          where T : IEMEvent
        {
            try
            {
                var eventPropJson = GeneretePropertiesJson(eMEvent.EventData);
                var checkResult = await _eventInfoLoader.GetOrAddEMEventInfoAsync(eMEvent, eventPropJson);
                if (checkResult != null)
                {
                    if (checkResult.EventPropertiesJson != eventPropJson)//if event has different properties .Throw Exception
                        throw new EMEventInvalidPropertyException($"{eMEvent.EventName} named event has different properties. Registered properties: {checkResult.EventPropertiesJson}, Sended properties {eventPropJson}");

                    var checkedAndCachedEventsDic = await GetAllEventsDicAsync();
                    if (checkedAndCachedEventsDic == null)
                        checkedAndCachedEventsDic = new Dictionary<string, EMEventInfoDto>();

                    checkedAndCachedEventsDic.Add(checkResult.EventName, checkResult);

                    _cacheManager.Set(_options.RegisteredEventsMemoryCacheKey, checkedAndCachedEventsDic, TimeSpan.FromMilliseconds(_options.CacheExpireTimeMinute));
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
        private async Task<Dictionary<string, EMEventInfoDto>> LoadAllRegisteredEventsAsync()
        {
            try
            {
                var list = await _eventInfoLoader.GetAllRegisteredEventsAsync();

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
        private async Task<Dictionary<string, EMEventInfoDto>> GetAllEventsDicAsync()
        {
            if (_cacheManager.TryGetValue(_options.RegisteredEventsMemoryCacheKey, out Dictionary<string, EMEventInfoDto> eventsDic))
                return eventsDic;
            else
                return await LoadAllRegisteredEventsAsync();

        }
        #endregion Async Checker


    }
}
