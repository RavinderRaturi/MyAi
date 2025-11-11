using Microsoft.Extensions.Logging.Abstractions;
using MyAi.Infrastructure.Services;

namespace MyAi.Tests
{
    [TestFixture]
    public class MockModelClientTests
    {
        private MockModelClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Use a NullLogger so tests don't need real logging infrastructure
            var logger = new NullLogger<MockModelClient>();
            _client = new MockModelClient(logger);
        }

        [Test]
        public async Task GenerateAsync_EchoesPrompt()
        {
            var options = new ModelRequestOptions
            {
                Temperature = 0.5,
                MaxTokens = 50
            };

            var res = await _client.GenerateAsync("hello", options);
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Text);
            Assert.IsTrue(res.Text.Contains("hello"));
        }
    }
}
