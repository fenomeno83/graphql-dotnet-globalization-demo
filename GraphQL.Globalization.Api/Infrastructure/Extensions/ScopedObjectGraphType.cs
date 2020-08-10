using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.Infrastructure.Extensions
{
    public class ScopedObjectGraphType : ObjectGraphType
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public ScopedObjectGraphType(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected T GetService<T>()
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetService<T>();
        }
    }
}
