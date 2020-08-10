using GraphQL.Types;
using GraphQL.Globalization.Api.GraphQLSchema.Extensions;
using GraphQL.Globalization.Entities.Models.Generic;
using GraphQL.Globalization.Entities.Models.Test;

namespace GraphQL.Globalization.Api.GraphQLSchema.Types
{
    //automatic graphql input and output type generator from dto. Is suggested register a "type" and a "input type" for each DTO   

    //public class TestResponseType : GraphQLGenericType<TestResponse>
    //{

    //    //example if you want add others graphql props to automatic generated props from dto
    //    public TestResponseType()
    //    {
    //        //Computed field example
    //        Field<StringGraphType>("otherCode", resolve: context => $"{context.Source.Code}-append-other");
    //    }
    //}
    public class TestResponseType : GraphQLGenericType<TestResponse> { }
    public class TestRequestInputType : GraphQLInputGenericType<TestRequest> { }

}
