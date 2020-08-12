using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.GraphQLSchema.Extensions
{
    public static class ServiceScopeExtensions
    {
        public static T GetService<T>(this IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
