using EventManager.EventChecker.Dto;
using System;

namespace EventManager.EventChecker.Extensions
{
    public static class EventCheckerOptionsExtensions
    {
        public static EventCheckerOptions UseSQL(this EventCheckerOptions opt, Action<Microsoft.EntityFrameworkCore.DbContextOptionsBuilder> sqlOptionsAction)
        {
            opt.UseWebApi = false;

            opt.UseSQL = true;
            opt.SqlOptions = sqlOptionsAction;

            return opt;
        }
        //TODO: implement web api
        //public static EventCheckerOptions UseWebApi(this EventCheckerOptions opt, Action<WebApiOptions> webApiOptionsAction)
        //{
        //    opt.UseSQL = false;

        //    opt.UseWebApi = true;
        //    opt.WebApiOptions = webApiOptionsAction;

        //    return opt;
        //}
    }
}
