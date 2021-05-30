using System.Linq;
using System.Reflection;
using GraphiQl;
using GraphQL.Types;
using GraphQL.Globalization.Api.GraphQLSchema;
using GraphQL.Globalization.Api.Infrastructure.Middlewares;
using GraphQL.Globalization.Entities;
using GraphQL.Globalization.Interfaces;
using GraphQL.Globalization.Services.Infrastructure.Extensions;
using GraphQL.Globalization.Services.Infrastructure.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GraphQL;
using GraphQL.AutoTypes;
using GraphQL.SystemTextJson;

namespace GraphQL.Globalization.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
           
         
            //GraphQL Types 
            //Scan the assembly where the GraphQL schema is defined for types,
            //foreach type register a singleton in IoC container
            (from t in Assembly.GetAssembly(typeof(GraphQLMainSchema)).GetTypes()
             where t.BaseType.IsGenericType &&
             (t.BaseType.GetGenericTypeDefinition() == typeof(GraphQLGenericType<>) ||
             t.BaseType.GetGenericTypeDefinition() == typeof(GraphQLInputGenericType<>))
             select t)
            .ToList()
            .ForEach(t => services.AddSingleton(t));

            //GraphQL Queries and Mutations
            //Scan the assembly where the GraphQL schema is defined for queries and mutations,
            //foreach type register a singleton in IoC container
            (from t in Assembly.GetAssembly(typeof(GraphQLMainSchema)).GetTypes()
             where !t.IsAbstract && !t.IsInterface &&
             (t.IsSubclassOf(typeof(ObjectGraphType)) ||
              t.IsSubclassOf(typeof(ObjectGraphType<>)))
             select t)
           .ToList()
           .ForEach(t => services.AddSingleton(t));


            services.AddSingleton<ISchema, GraphQLMainSchema>();

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddControllers()
                    .AddNewtonsoftJson()
                    .AddDataAnnotationsLocalization //model data annotation/validation using localization resources
                    (
                        options =>
                        {
                            options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Resources));
                        }
                    );


            

            services.AddLocalization(o =>
            {
                // localization in separated assembly
                o.ResourcesPath = "Resources";
            });


            services.AddHttpContextAccessor();

            services.AddBusinessServices();

            services.AddDbContext(Configuration); 


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            app.UseGraphiQl();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseBaseMiddleware();

            //give services access also to static methods, where you can't use dependency injection. You shouldn't use it. Prefer dependecy injection and services methods
            //AppServicesHelper.Services = app.ApplicationServices;

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(cpb =>
            {
                cpb.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();


            });

            app.UseLocalizationMiddleware(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });


        }
    }
}
