using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api
{
    public static class ApiMessages
    {
        public const string ApiMethodLog = "ID: {0}. Called {1} with execution time {2} ms";
        public const string ApiErrorLog = "ID: {0}. Error for method {1} with execution time {2} ms and request:\n{3}";
        public const string ApiMethodLogWithBodyRequest = "ID: {0}. Called {1} with execution time {2} ms and request:\n{3}";
        public const string ApiErrorLogWithBodyRequest = "ID: {0}. Error for method {1} with execution time {2} ms";
        public const string NotValidModel = "Input model is not valid with errors:\n{0}";

    }
    
}
