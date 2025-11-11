using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using MyAi.Infrastructure.Services;

namespace MyAi.Tests
{
    [TestFixture]
    public class PromptTemplateTests
    {
        private string _tempDir;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        [Test]
        public async Task RenderAsync_RendersTemplateVariables()
        {
            // Arrange
            var filePath = Path.Combine(_tempDir, "test-template.md");
            await File.WriteAllTextAsync(filePath, "Hello {{ name }}, age {{ age }}.");

            var service = new ScribanPromptTemplateService(_tempDir);
            var model = new Dictionary<string, object>
            {
                ["name"] = "Ravi",
                ["age"] = 30
            };

            // Act
            var result = await service.RenderAsync("test-template.md", model);

            // Assert
            Assert.That(result.Trim(), Is.EqualTo("Hello Ravi, age 30."));
        }

        [Test]
        public void RenderAsync_ThrowsIfTemplateMissing()
        {
            var service = new ScribanPromptTemplateService(_tempDir);
            Assert.ThrowsAsync<FileNotFoundException>(async () =>
                await service.RenderAsync("nonexistent.md", new Dictionary<string, object>())
            );
        }

        [Test]
        public async Task RenderAndCallLocalProvider_WhenEnabled()
        {
            var run = Environment.GetEnvironmentVariable("RUN_INTEGRATION_TESTS");
            Console.WriteLine($"RUN_INTEGRATION_TESTS env: '{run ?? "<null>"}'");

            if (run != "1")
            {
                Assert.Ignore("Integration tests disabled; set RUN_INTEGRATION_TESTS=1");
                return;
            }

            // ... rest of test
        }
        [Test]
        public async Task RenderAsync_LoadsRealChatTemplate()
        {
            var root = Path.Combine(TestContext.CurrentContext.TestDirectory, "prompts");
            var service = new ScribanPromptTemplateService(root);

            var model = new Dictionary<string, object> { ["user_instructions"] = "Say hello" };
            var result = await service.RenderAsync("chat.prompt.v1.md", model);

            Assert.That(result.Contains("Say hello"));
            Assert.That(result.Contains("# System"));
        }


    }
}
