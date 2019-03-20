using FBM.Event.UniqueController.Data.dbEntities;
using FBM.Event.UniqueController.Dto;
using FBM.Event.UniqueController.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.UniqueController.Extensions
{
    public static class EventUniqueControllerDIExtensions
    {
        public static IServiceCollection AddEventChecker(this IServiceCollection collection, Action<EventCheckerOptions> optionsAction)
        {
            var options = new EventCheckerOptions();
            optionsAction.Invoke(options);

            if (options.UseSQL)
            {
                if (options.SqlOptions == null)
                    throw new Exception("Define SQLOptions to use sql");

                collection.AddDbContext<UniqueControllerDbContext>(options.SqlOptions);
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
