using System.Threading;
using System.Threading.Tasks;

namespace MyAi.Core.Services
{
    public class ModelResponse { public string Text { get; set; } = string.Empty; }

    public interface IModelClient
    {
        Task<ModelResponse> GenerateAsync(string prompt, CancellationToken ct = default);
    }
}
