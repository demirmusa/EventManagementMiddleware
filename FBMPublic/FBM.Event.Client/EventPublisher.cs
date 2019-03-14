using FBM.Event.Client.Dto;
using FBM.Event.Client.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Client
{
    /// <summary>
    /// this use TPublisher , inject it to dependency injection and initialize it before use call publish method
    /// inject this as a singleteon
    /// </summary>
    public class EventPublisher<TPublisher> : IEventPublisher where TPublisher : IMessagePublisher
    {
        IEventManager _eventManager;
        TPublisher _genericPublisher;
        public EventPublisher(TPublisher genericPublisher, IEventManager eventManager)
        {
            _genericPublisher = genericPublisher;
            _eventManager = eventManager;
        }


        public void Publish<T>(T nodeEvent) where T : IFBMEvent
        {
            FBMEvent<T> fbmNodeEvent;
            try
            {
                fbmNodeEvent = _eventManager.GetEvent(nodeEvent);
            }
            catch (Exception e)
            {
                throw new Exception("Error while event info. For more information see inner exception", e);
            }

            string msgStr = "";
            try
            {
                msgStr = Newtonsoft.Json.JsonConvert.SerializeObject(fbmNodeEvent);
            }
            catch (Exception e)
            {
                throw new Exception("Error while serializing object to json. For more information see inner exception", e);
            }


            try
            {
                _genericPublisher.Publish(msgStr);
            }
            catch (Exception e)
            {
                throw new Exception("Error while publishing message. For more information see inner exception", e);
            }
        }

    }
}
