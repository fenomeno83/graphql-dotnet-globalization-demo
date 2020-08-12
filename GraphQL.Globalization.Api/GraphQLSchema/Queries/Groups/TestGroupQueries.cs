using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using GraphQL.Globalization.Api.GraphQLSchema.Types;
using GraphQL.Globalization.Api.Infrastructure.Extensions;
using GraphQL.Globalization.Common.Extensions;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using GraphQL.Globalization.Api.GraphQLSchema.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Globalization.Api.GraphQLSchema.Queries.Groups
{

    public class TestGroupQueries : ObjectGraphType
    {
        public TestGroupQueries(IHttpContextAccessor _httpContextAccessor)
        {
            Name = "testQueries";

            #region example
            /*
             query($input: Int!)
             {
                testQueries
                {
                    demoQuery(input: $input)
                    {
                        code
                    }
                }
             }


             {
                "input": 1
             }
            */
            #endregion
            FieldAsync<TestResponseType>(
                "demoQuery",
                    arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "input" }),
                    resolve: async context =>
                    {
                        using (var scope = _httpContextAccessor.CreateScope())
                        {
                            //example of scopes authorization, if it is configured from startup.cs
                            //_httpContextAccessor.HttpContext.ValidateScopes("read");

                            return await scope.GetService<ITestService>().DemoQuery(context.GetArgument<int>("input"));
                        }
                    });

           
        }
    }
}
