using FBM.Event.UniqueController.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.UniqueController.Extensions
{
    public static class UniqueControllerOptionsExtensions
    {
        public static EventCheckerOptions UseSQL(this EventCheckerOptions opt, Action<Microsoft.EntityFrameworkCore.DbContextOptionsBuilder> sqlOptionsAction)
        {
            opt.UseWebApi = false;

            opt.UseSQL = true;
            opt.SqlOptions = sqlOptionsAction;

            return opt;
        }

        public static EventCheckerOptions UseWebApi(this EventCheckerOptions opt, Action<WebApiOptions> webApiOptionsAction)
        {
            opt.UseSQL = false;

            opt.UseWebApi = true;
            opt.WebApiOptions = webApiOptionsAction;

            return opt;
        }
    }
}
