using EventManager.EventChecker.SQL.Data;
using EventManager.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EventManager.EventChecker.SQL
{
    public static class EventCheckerSQLDIExtensions
    {
        public static IServiceCollection UseSQLChecker(this IServiceCollection services, Action<Microsoft.EntityFrameworkCore.DbContextOptionsBuilder> dbContextOptionsBuilderAction)
        {

            services.AddDbContext<EventCheckerDbContext>(dbContextOptionsBuilderAction);
            services.AddTransient<IEventInfoLoader, SQLEventChecker>();

            return services;
        }
    }
}
