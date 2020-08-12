using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.GraphQLSchema.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static IServiceScope CreateScope(this IHttpContextAccessor _httpContextAccessor)
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IContextService>().CreateScope();
        }
    }
}
