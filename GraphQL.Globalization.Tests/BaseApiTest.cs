using GraphQL.Globalization.Api;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Globalization.Tests
{
    public class BaseApiTest
    {
        protected readonly HttpClient _apiClient;
        protected readonly ITestService _testService;
        protected readonly IInfrastructureService _infrastructure;

        public BaseApiTest()
        {
            // Arrange test enviromnent
            var builder = new WebHostBuilder();
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile(
                    $"config/appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                    optional: true);
                config.AddEnvironmentVariables();
            })
                .UseStartup<Startup>();

            // Load environment variables
            LoadEnvironmentVariables();

            // Start test server
            TestServer testServer = new TestServer(builder);
            _apiClient = testServer.CreateClient();

            // Set services
            _testService = testServer.Services.GetService<ITestService>();
            _infrastructure = testServer.Services.GetService<IInfrastructureService>();

            // Crate fake HttpContext
            CreateDefaultHttpContext();
        }

        protected void LoadEnvironmentVariables()
        {
            using (var file = File.OpenText("Properties/launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }

        protected void CreateDefaultHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/TestingUrl";
            context.Request.Host = new HostString("localhost:4040");
            context.Request.Scheme = "http";
            context.Request.Method = "GET";

            var contextAccessor = new HttpContextAccessor
            {
                HttpContext = context
            };
        }
    }
}
