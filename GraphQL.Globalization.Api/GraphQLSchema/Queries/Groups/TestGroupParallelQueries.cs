using GraphQL.Globalization.Api.GraphQLSchema.Types;
using GraphQL.Globalization.Api.Infrastructure.Extensions;
using GraphQL.Globalization.Interfaces;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.GraphQLSchema.Queries.Groups
{
    public class TestGroupParallelQueries : ScopedObjectGraphType
    {
        public TestGroupParallelQueries(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            Name = "testParallelQueries";

            #region example
            /*
             query($input: Int!)
             {
                testParallelQueries
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

                        return await GetService<ITestParallelService>().DemoQuery(context.GetArgument<int>("input"));
                    });

            #region example
            /*
             query($input: Int!)
             {
                testParallelQueries
                {
                    demoOtherQuery(input: $input)
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
                "demoOtherQuery",
                    arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "input" }),
                    resolve: async context =>
                    {
                        //example of scopes authorization, if it is configured from startup.cs
                        //_httpContextAccessor.HttpContext.ValidateScopes("read");

                        return await GetService<ITestParallelService>().DemoOtherQuery(context.GetArgument<int>("input"));
                    });


        }
    }
}
