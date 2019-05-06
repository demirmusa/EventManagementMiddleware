using EventManager.Core.Interfaces;
using EventManager.EventChecker;
using EventManager.EventChecker.Interfaces;
using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Core.Extensions
{
    public static class EventManagerCoreDIExtensions
    {
        public static IServiceCollection AddEventManagerCore<TPublisher, TCacheManager>(this IServiceCollection collection,
            Action<EventManagerOptions> eventManagerOptionsAction = null
            )
            where TPublisher : IMessagePublisher
            where TCacheManager : ICacheManager
        {
            eventManagerOptionsAction = eventManagerOptionsAction ?? (x => { });
            collection.Configure(eventManagerOptionsAction);

            collection.AddTransient<IEventChecker, EventChecker<TCacheManager>>();           
            collection.AddTransient<IEMPublisher, EventPublisherCore<TPublisher>>();

            EventManagerOptions opt = new EventManagerOptions();
            eventManagerOptionsAction.Invoke(opt);
            if (opt.CheckIsEventUnique)
                collection.AddTransient<IEventManagerCore, EventManagerWithChecker>();
            else
                collection.AddTransient<IEventManagerCore, EventManagerCore>();


            return collection;
        }
    }

}
