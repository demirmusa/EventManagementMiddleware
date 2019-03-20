using EventManager.Core.Dto;
using EventManager.Core.interfaces;
using EventManager.EventChecker.Dto;
using EventManager.EventChecker.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Core.Extensions
{
    public static class EventManagerCoreDIExtensions
    {
        public static IServiceCollection AddEventManagerCore<TPublisher, TCacheManager>(this IServiceCollection collection,
            Action<EventManagerOptions> eventManagerOptionsAction,
            Action<EventCheckerOptions> eventCheckerOptionsAction)
            where TPublisher : IMessagePublisher
            where TCacheManager : ICacheManager
        {
            collection.Configure(eventManagerOptionsAction);

            collection.AddTransient<IEventManagerCore, EventManagerCore<TCacheManager>>();
            collection.AddTransient<IEMPublisher, EventPublisherCore<TPublisher>>();

            collection.AddEventChecker(eventCheckerOptionsAction);

            return collection;
        }
    }

}
