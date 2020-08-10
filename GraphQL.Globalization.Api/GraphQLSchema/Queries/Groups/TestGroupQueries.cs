using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using GraphQL.Globalization.Api.GraphQLSchema.Types;
using GraphQL.Globalization.Api.Infrastructure.Extensions;
using GraphQL.Globalization.Common.Extensions;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace GraphQL.Globalization.Api.GraphQLSchema.Queries.Groups
{

    public class TestGroupQueries : ScopedObjectGraphType
    {
        public TestGroupQueries(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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
                        //example of scopes authorization, if it is configured from startup.cs
                        //_httpContextAccessor.HttpContext.ValidateScopes("read");

                        return await GetService<ITestService>().DemoQuery(context.GetArgument<int>("input"));
                    });


        }
    }
}
