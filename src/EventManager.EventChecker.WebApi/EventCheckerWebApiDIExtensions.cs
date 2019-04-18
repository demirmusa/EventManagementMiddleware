using EventManager.EventChecker.WebApi.Dto;
using EventManager.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EventManager.EventChecker.WebApi
{
    public static class EventCheckerWebApiDIExtensions
    {
        public static IServiceCollection UseWebApiChecker(this IServiceCollection services, Action<WebApiOptions> webApiOptionsAction)
        {
            throw new NotImplementedException("The option to check from web api has not yet been implemented.");
            services.AddTransient<IEventInfoLoader, WebApiEventChecker>();
            services.Configure(webApiOptionsAction);
            return services;
        }
    }
}
