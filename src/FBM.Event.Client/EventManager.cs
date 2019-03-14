using FBM.Event.Client.Dto;
using FBM.Event.Client.interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FBM.Event.Client
{
    public class EventManager<TCacheManager> : IEventManager where TCacheManager : ICacheManager
    {
        TCacheManager _cacheManager;
        EventManagerOptions _options;
        public EventManager(TCacheManager cacheManager, IOptions<EventManagerOptions> options)
        {
            _cacheManager = cacheManager;
            if (options.Value == null)
                throw new Exception("EventManagerOptions can not be null");

            if (options.Value.CheckEventOnServer && string.IsNullOrEmpty(options.Value.EventApiServerAddress))
                throw new Exception("EventApiServerAddress can not be null");

            if (string.IsNullOrEmpty(options.Value.RegisteredEventsMemoryCacheKey))
                options.Value.RegisteredEventsMemoryCacheKey = "EventManager.RegisteredEvents";

            _options = options.Value;
        }

        public FBMEvent<T> GetEvent<T>(T e) where T : IFBMEvent
        {
            string eventName = GetEventName(e.GetType());

            if (_options.CheckEventOnServer)
                CheckEvent(e.GetType(), eventName);

            return new FBMEvent<T>()
            {
                EventData = e,
                EventName = eventName
            };
        }
        private void CheckEvent(Type t, string eventName)
        {
            if (_cacheManager.TryGetValue(_options.RegisteredEventsMemoryCacheKey, out Dictionary<string, FBMEventServerDto> checkedAndCachedEventsDic))
            {
                if (!checkedAndCachedEventsDic.ContainsKey(eventName))// if dictionary contains event info , it means we already check it and add it to dictionary                  
                    CheckEventFromServer(t, eventName, checkedAndCachedEventsDic);
                //else
                //{
                //    //do nothing. this event is in checked list 
                //}
            }
            else
                CheckEventFromServer(t, eventName);
        }
        


        private string GetEventName(Type t)
        {
            var attributes = t.GetCustomAttributes();
            foreach (var attr in attributes)
            {
                // if class contains FBMEventInfoAttribute take event name from attribute
                if (attr is FBMEventInfoAttribute eventInfoAttribute)
                    return eventInfoAttribute.EventName;
            }
            return t.Name;//otherwise event name is class name
        }

        //TODO: extend
        private void CheckEventFromServer(Type t, string eventName, Dictionary<string, FBMEventServerDto> checkedAndCachedEventsDic = null)
        {

            throw new NotImplementedException("");

            var client = new RestClient(_options.EventApiServerAddress);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("FBMEvents", Method.POST);
            //send event with properties to server , server will check it if its ok it will return 
            request.AddJsonBody(new { eventProperties = "", eventName });

            IRestResponse response = client.Execute(request);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //TODO: Rewrite here.
                //check response
                CheckServerResponse(response);

                if (string.IsNullOrEmpty(response.Content))
                    throw new Exception("Error: Event check server return status code 200 with no content.");


                if (checkedAndCachedEventsDic == null)
                    checkedAndCachedEventsDic = new Dictionary<string, FBMEventServerDto>();

                checkedAndCachedEventsDic.Add(eventName, Newtonsoft.Json.JsonConvert.DeserializeObject<FBMEventServerDto>(response.Content));

                _cacheManager.Set(_options.RegisteredEventsMemoryCacheKey, checkedAndCachedEventsDic,
                    TimeSpan.FromMinutes(_options.CacheExpireTimeMinute));
            }
            else
                throw new Exception($"Error while sending event to server. Status Code: {response.StatusCode} , Content: {response.Content}");

        }
        private void CheckServerResponse(IRestResponse response)
        {
            //TODO: check response
        }
    }
}
