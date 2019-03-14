using FBM.Event.Client.Dto;
using FBM.Event.Client.interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Client.Extensions
{
    public static class FBMEventClientDIExtensions
    {
        public static IServiceCollection AddFBMEventClient<TPublisher, TCacheManager>(this IServiceCollection collection, Action<EventManagerOptions> eventManagerOptions)
            where TPublisher : IMessagePublisher
            where TCacheManager : ICacheManager
        {
            collection.Configure(eventManagerOptions);

            collection.AddTransient<IEventManager, EventManager<TCacheManager>>();
            collection.AddTransient<IEventPublisher, EventPublisher<TPublisher>>();

            return collection;
        }
    }

}
