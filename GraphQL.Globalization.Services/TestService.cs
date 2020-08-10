using GraphQL.Globalization.Data;
using GraphQL.Globalization.Entities.Models.Test;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static GraphQL.Globalization.Entities.Models.Enums;

namespace GraphQL.Globalization.Services
{
    public class TestService : ServiceBase, ITestService
    {
        public TestService(IInfrastructureService infrastructure, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db)
            : base(infrastructure, httpContextAccessor, db)
        {
        }

        public async Task<TestResponse> DemoMutation(TestRequest request)
        {
            //emulate an async db operation
            await _db.SaveChangesAsync();

            return new TestResponse()
            {
                Code = Guid.NewGuid().ToString()
            };

        }

        public async Task<TestResponse> DemoQuery(int id)
        {
            //emulate an async operation
            await Task.Delay(1);

            return new TestResponse()
            {
                Code = Guid.NewGuid().ToString()
            };
        }
    }
}

