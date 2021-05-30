using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using GraphQL.Globalization.Api.GraphQLSchema;
using GraphQL.Globalization.Entities;
using GraphQL.Globalization.Entities.Extensions;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.Infrastructure.Extensions
{
    public static class GraphQLExtensions
    {

        public static T GetArgumentExtension<T>(this IResolveFieldContext context, string name)
        {

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(context.GetArgument<dynamic>(name)));

        }

    }
}
