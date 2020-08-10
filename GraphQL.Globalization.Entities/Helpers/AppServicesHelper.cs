using Microsoft.Extensions.Localization;
using System;

namespace GraphQL.Globalization.Entities.Helpers
{
    public static class AppServicesHelper
    {
        private static IServiceProvider services = null;

        public static IServiceProvider Services
        {
            get { return services; }
            set
            {
                if (services != null)
                {
                    throw new Exception(Messages.NoMoreThanOneSPInstance);
                }
                services = value;
            }
        }

        public static IStringLocalizer StringLocalizer
        {
            get
            {
                return services.GetService(typeof(IStringLocalizer<Resources>)) as IStringLocalizer<Resources>;
            }
        }

        //add other static methods if you want give access to other services like HttpContext, IHostingEnvironment 
    }
}

