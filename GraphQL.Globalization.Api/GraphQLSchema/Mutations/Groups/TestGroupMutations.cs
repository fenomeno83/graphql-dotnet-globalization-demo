using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using GraphQL.Globalization.Api.GraphQLSchema.Types;
using GraphQL.Globalization.Api.Infrastructure.Extensions;
using GraphQL.Globalization.Common.Extensions;
using GraphQL.Globalization.Entities.Models.Test;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.Globalization.Services.Infrastructure.Managers;
using GraphQL.Globalization.Api.GraphQLSchema.Extensions;

namespace GraphQL.Globalization.Api.GraphQLSchema.Mutations.Groups
{
    public class TestGroupMutations : ObjectGraphType
    {
        public TestGroupMutations(IHttpContextAccessor _httpContextAccessor)
        {

            Name = "testMutations";
            #region example
            /*
                mutation($input: TestRequestInput!)
                {
                    testMutations
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
                        using (var scope = _httpContextAccessor.CreateScope())
                        {

                            //example of scopes authorization, if it is configured from startup.cs
                            //_httpContextAccessor.HttpContext.ValidateScopes("create");

                            //get dto; use GetArgumentExtension instead of native GetArgument, because solve some deserialization problems
                            TestRequest request = context.GetArgumentExtension<TestRequest>("input");

                            //validate model
                            Infrastructure.Extensions.Validation.Validate(request, _httpContextAccessor.HttpContext);

                            return await scope.GetService<ITestService>().DemoMutation(request);

                            //return await GetService<ITestService>().DemoMutation(request);
                        }

                    });

            
        }
    }
}
