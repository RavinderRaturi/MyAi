using NUnit.Framework;
using MyAi.Infrastructure.Services;
using System.Threading.Tasks;

namespace MyAi.Tests
{
    public class MockModelClientTests
    {
        [Test]
        public async Task GenerateAsync_EchoesPrompt()
        {
            var client = new MockModelClient();
            var res = await client.GenerateAsync("hello");
            Assert.IsTrue(res.Text.Contains("hello"));
        }
    }
}
