using GraphQL.Globalization.Data;
using GraphQL.Globalization.Interfaces;
using GraphQL.Globalization.Services.Infrastructure.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Services.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("DB")));
        }

        public static void AddBusinessServices(this IServiceCollection services)
        {

            // Business services
            services.AddSingleton<IEnumsManager, EnumsManager>();
            services.AddSingleton<ILogService, LogService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IInfrastructureService, InfrastructureService>();
            services.AddScoped<IValidationContextService, ValidationContextService>();


        }
    }
}
