using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GraphQL.Globalization.Common;
using GraphQL.Globalization.Common.Extensions;
using GraphQL.Globalization.Entities.Models.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static GraphQL.Globalization.Entities.Models.Enums;

namespace GraphQL.Globalization.Api.Infrastructure.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BaseMiddleware
    {
        private readonly RequestDelegate _next;

        public BaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration)
        {

            //enable request body access
            context.Request.EnableBuffering();

            await _next(context);


        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BaseMiddlewareExtensions
    {
        public static IApplicationBuilder UseBaseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BaseMiddleware>();
        }
    }
}
