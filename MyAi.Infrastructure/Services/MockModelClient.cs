using Microsoft.Extensions.Logging;
using MyAi.Core.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MyAi.Infrastructure.Services
{
    public class MockModelClient : IModelClient
    {
        private readonly ILogger<MockModelClient> _logger;
        public string Name => "Mock";

        public MockModelClient(ILogger<MockModelClient> logger) { _logger = logger; }


     

        public Task<ModelResponse> GenerateAsync(string prompt, ModelRequestOptions options, CancellationToken ct = default)
        {
            var est = TokenEstimator.Estimate(prompt);
            _logger.LogDebug("Mock provider. Len:{Len}, TokensEst:{Est}, Options:{@Opts}", prompt?.Length, est, options);
            var text = "[mock] " + (prompt?.Length > 200 ? prompt.Substring(0, 200) + "..." : prompt);
            return Task.FromResult(new ModelResponse { Text = text });
        }
    }
}
