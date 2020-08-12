using GraphQL.Types;
using GraphQL.Globalization.Api.GraphQLSchema.Queries.Groups;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.GraphQLSchema.Queries
{

    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            //add all group queries here
            FieldAsync<TestGroupQueries>("testQueries", resolve: async context => await Task.FromResult(new { }));
            FieldAsync<TestGroupParallelQueries>("testParallelQueries", resolve: async context => await Task.FromResult(new { }));

        }
    }

}
