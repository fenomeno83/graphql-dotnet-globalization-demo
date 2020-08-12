using GraphQL.Globalization.Data;
using GraphQL.Globalization.Entities.Models.Test;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Services
{
    public class TestParallelService : ServiceBaseParallel, ITestParallelService
    {
        public TestParallelService(IInfrastructureService infrastructure, IHttpContextAccessor httpContextAccessor, Func<ApplicationDbContext> dbfunc)
            : base(infrastructure, httpContextAccessor, dbfunc)
        {
        }

       
        public async Task<TestResponse> DemoMutation(TestRequest request)
        {
            using (var _db = _dbfunc())
            {
                return await DemoMutationPassedContext(request, _db);
            }

        }

        public async Task<TestResponse> DemoMutationPassedContext<T>(TestRequest request, T context = null) where T : class
        {
            ApplicationDbContext _db = null;
            var cont = context as ApplicationDbContext;
            if (cont != null)
                _db = cont;

            //conditional using; if context is passed, "using" is null (it's the same as not using "using"), otherwise instantiate new dbcontext from factory
            using (cont == null
               ? _db = _dbfunc()
               : null)
            {
                await _db.SaveChangesAsync();

                return new TestResponse()
                {
                    Code = Guid.NewGuid().ToString()
                };
            }
        }

        public async Task<TestResponse> DemoQuery(int id)
        {
            using (var _db = _dbfunc())
            {
                //emulate an async operation
                await Task.Delay(1);

                return new TestResponse()
                {
                    Code = Guid.NewGuid().ToString()
                };
            }
        }

        public async Task<TestResponse> DemoOtherQuery(int id)
        {
            using (var _db = _dbfunc())
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
}
