using System.Threading;
using System.Threading.Tasks;

namespace MyAi.Core.Services
{
    public class ModelResponse { public string Text { get; set; } = string.Empty; }


    public interface IModelClient
    {
        Task<ModelResponse> GenerateAsync(string prompt, ModelRequestOptions options, CancellationToken ct = default);
        string Name { get; }
    }
}
