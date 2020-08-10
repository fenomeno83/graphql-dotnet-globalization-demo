using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GraphQL.Globalization.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config.SetBasePath(Directory.GetCurrentDirectory());
                   config.AddJsonFile(Path.Join("config", "appsettings.json"), optional: true, reloadOnChange: true);
                   config.AddJsonFile(Path.Join("config", $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json"), optional: true);
                   config.AddEnvironmentVariables();
               })
               .UseStartup<Startup>();
    }
}
