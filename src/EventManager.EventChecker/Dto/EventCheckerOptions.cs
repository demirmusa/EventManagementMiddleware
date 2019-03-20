using System;
namespace EventManager.EventChecker.Dto
{
    public class EventCheckerOptions
    {
        internal bool UseSQL { get; set; } = true;
        internal Action<Microsoft.EntityFrameworkCore.DbContextOptionsBuilder> SqlOptions { get; set; }

        internal bool UseWebApi { get; set; }

        internal Action<WebApiOptions> WebApiOptions { get; set; }
    }
}
