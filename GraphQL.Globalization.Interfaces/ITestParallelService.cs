using GraphQL.Globalization.Entities.Models.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Interfaces
{
    public interface ITestParallelService
    {
        Task<TestResponse> DemoMutation(TestRequest request);
        Task<TestResponse> DemoMutationPassedContext<T>(TestRequest request, T context = null) where T : class;
        Task<TestResponse> DemoQuery(int id);
    }
}
