using System;

namespace GraphQL.Globalization.Api.GraphQLSchema.Extensions
{
    public class GraphQLSchemaException : ApplicationException
    {
        string _genericTypeName = null;

        public GraphQLSchemaException() : base() { }
        public GraphQLSchemaException(string genericTypeName) : base() { }
        public GraphQLSchemaException(string genericTypeName, string message) : base(message: message) { }
        public GraphQLSchemaException(string genericTypeName, string message, Exception innerException) : base(message: message, innerException: innerException) { }

        public string GenericTypeName => _genericTypeName;
    }
}