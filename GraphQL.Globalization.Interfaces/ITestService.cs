using GraphQL.Globalization.Entities.Models.Test;
using System;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Interfaces
{
    public interface ITestService
    {
        Task<TestResponse> DemoMutation(TestRequest request);
        Task<TestResponse> DemoQuery(int id);
    }
}
