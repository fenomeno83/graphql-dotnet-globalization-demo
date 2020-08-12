using GraphQL.Globalization.Api.GraphQLSchema.Types;
using GraphQL.Globalization.Api.Infrastructure.Extensions;
using GraphQL.Globalization.Entities.Models.Test;
using GraphQL.Globalization.Interfaces;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.GraphQLSchema.Mutations.Groups
{
    public class TestGroupParallelMutations : ScopedObjectGraphType
    {
        public TestGroupParallelMutations(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

            Name = "testParallelMutations";
            #region example
            /*
                mutation($input: TestRequestInput!)
                {
                    testParallelMutations
                    {
                        demoMutation(input: $input)
                        {
                            code
                        }
                    }
                }


                {
                    "input": 
                    {
                        "fake": "fake text",
                        "fakeEnum": 100,
                        "test": 1,
                        "testOther": 2
                    }
                }
            */
            #endregion
            FieldAsync<TestResponseType>(
                "demoMutation",
                    arguments: new QueryArguments(new QueryArgument<NonNullGraphType<TestRequestInputType>> { Name = "input" }),
                    resolve: async context =>
                    {
                        //example of scopes authorization, if it is configured from startup.cs
                        //_httpContextAccessor.HttpContext.ValidateScopes("create");

                        //get dto; use GetArgumentExtension instead of native GetArgument, because solve some deserialization problems
                        TestRequest request = context.GetArgumentExtension<TestRequest>("input");

                        //validate model
                        Infrastructure.Extensions.Validation.Validate(request, httpContextAccessor.HttpContext);

                        return await GetService<ITestParallelService>().DemoMutation(request);
                    });

            #region example
            /*
                mutation($input: TestRequestInput!)
                {
                    testParallelMutations
                    {
                        demoMutationPassedContext(input: $input)
                        {
                            code
                        }
                    }
                }


                {
                    "input": 
                    {
                        "fake": "fake text",
                        "fakeEnum": 100,
                        "test": 1,
                        "testOther": 2
                    }
                }
            */
            #endregion
            FieldAsync<TestResponseType>(
                "demoMutationPassedContext",
                    arguments: new QueryArguments(new QueryArgument<NonNullGraphType<TestRequestInputType>> { Name = "input" }),
                    resolve: async context =>
                    {
                        //example of scopes authorization, if it is configured from startup.cs
                        //_httpContextAccessor.HttpContext.ValidateScopes("create");

                        //get dto; use GetArgumentExtension instead of native GetArgument, because solve some deserialization problems
                        TestRequest request = context.GetArgumentExtension<TestRequest>("input");

                        //validate model
                        Infrastructure.Extensions.Validation.Validate(request, httpContextAccessor.HttpContext);

                        return await GetService<ITestParallelService>().DemoMutationPassedContext<object>(request);
                    });

        }
    }
}
