using FBM.Event.Client.Dto;
using FBM.Event.Client.interfaces;
using FBM.Event.UniqueController.Dto;
using FBM.Event.UniqueController.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Client.Extensions
{
    public static class FBMEventClientDIExtensions
    {
        public static IServiceCollection AddFBMEventClient<TPublisher, TCacheManager>(this IServiceCollection collection,
            Action<EventManagerOptions> eventManagerOptionsAction,
            Action<EventCheckerOptions> eventCheckerOptionsAction)
            where TPublisher : IMessagePublisher
            where TCacheManager : ICacheManager
        {
            collection.Configure(eventManagerOptionsAction);

            collection.AddTransient<IEventManager, EventManager<TCacheManager>>();
            collection.AddTransient<IEventPublisher, EventPublisher<TPublisher>>();

            collection.AddEventChecker(eventCheckerOptionsAction);

            return collection;
        }
    }

}
