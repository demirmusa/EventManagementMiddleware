using EventManager.EventChecker.Data.dbEntities;
using EventManager.EventChecker.Dto;
using EventManager.EventChecker.interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EventManager.EventChecker.Extensions
{
    public static class EventCheckerDIExtensions
    {
        public static IServiceCollection AddEventChecker(this IServiceCollection collection, Action<EventCheckerOptions> optionsAction)
        {
            var options = new EventCheckerOptions();
            optionsAction.Invoke(options);

            if (options.UseSQL)
            {
                if (options.SqlOptions == null)
                    throw new Exception("Define SQLOptions to use sql");

                collection.AddDbContext<EventCheckerDbContext>(options.SqlOptions);
                collection.AddTransient<IEventChecker, SQLEventChecker>();
            }
            else if (options.UseWebApi)
            {
                if (options.WebApiOptions==null)
                    throw new Exception("Define WebApiOptions to use web api");

                collection.AddTransient<IEventChecker, WebApiEventChecker>();
            }

            return collection;
        }
    }
}
