using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using GraphQL.Globalization.Api.GraphQLSchema.Mutations;
using GraphQL.Globalization.Api.GraphQLSchema.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GraphQL.Globalization.Api.GraphQLSchema
{

    public class GraphQLMainSchema : Schema
    {

        public GraphQLMainSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<RootQuery>();
            Mutation = resolver.Resolve<RootMutation>();
        }

    }
}
