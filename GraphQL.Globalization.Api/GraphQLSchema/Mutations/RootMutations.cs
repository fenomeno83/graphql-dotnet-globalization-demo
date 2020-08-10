using GraphQL.Types;
using GraphQL.Utilities;
using GraphQL.Globalization.Api.GraphQLSchema.Mutations.Groups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.GraphQLSchema.Mutations
{

    public class RootMutation : ObjectGraphType
    {
        public RootMutation()
        {
            //add all group mutations here
            FieldAsync<TestGroupMutations>("testMutations", resolve: async context => await Task.FromResult(new { }));

        }
    }


}
