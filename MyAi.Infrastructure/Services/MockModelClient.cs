using MyAi.Core.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MyAi.Infrastructure.Services
{
    public class MockModelClient : IModelClient
    {
        public Task<ModelResponse> GenerateAsync(string prompt, CancellationToken ct = default)
        {
            return Task.FromResult(new ModelResponse { Text = $"[mock] {prompt}" });
        }
    }
}
