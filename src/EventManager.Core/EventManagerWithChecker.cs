using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventManager.EventChecker.Interfaces;
using EventManager.Shared.Dto;
using Microsoft.Extensions.Options;

namespace EventManager.Core
{
    public class EventManagerWithChecker : EventManagerCore
    {
        IEventChecker _eventChecker;
        public EventManagerWithChecker(IOptions<EventManagerOptions> options, IEventChecker eventChecker) : base(options)
        {
            _eventChecker = eventChecker;
        }

        public override EMEvent<T> GetEvent<T>(T e)
        {
            var newEvent = base.GetEvent(e);
            _eventChecker.CheckEvent(newEvent);
            return newEvent;
        }

        public override async Task<EMEvent<T>> GetEventAsync<T>(T e)
        {
            var newEvent = await base.GetEventAsync(e);
            await _eventChecker.CheckEventAsync(newEvent);
            return newEvent;
        }
    }
}
