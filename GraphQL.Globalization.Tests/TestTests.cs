using GraphQL.Globalization.Entities.Models.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using static GraphQL.Globalization.Entities.Models.Enums;

namespace GraphQL.Globalization.Tests
{
    [TestClass]
    public class TestTests : BaseApiTest
    {
        [TestMethod]
        public async Task DemoMutation()
        {
            var request = new TestRequest()
            {
                Fake = "fake",
                FakeOther = "fake other",
                Test = 1,
                TestOther = 2,
                FakeEnum = FakeEnum.FirstFake
            };

            var response = await _testService.DemoMutation(request);

            Assert.IsNotNull(response?.Code);
        }

        [TestMethod]
        public async Task DemoQuery()
        {

            var response = await _testService.DemoQuery(1);

            Assert.IsNotNull(response?.Code);
        }
    }
}

