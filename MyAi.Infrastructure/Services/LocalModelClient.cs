using MyAi.Core.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyAi.Infrastructure.Services
{
    public class LocalModelClient : IModelClient
    {
        private readonly HttpClient _http;
        private readonly string _model;
        private const string defaultModel = "phi4";

        public LocalModelClient(HttpClient httpClient, string model = defaultModel)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _model = model;
        }

        public async Task<ModelResponse> GenerateAsync(string prompt, CancellationToken ct = default)
        {
            var payload = new { model = _model, prompt = prompt, stream = false };
            var httpRes = await _http.PostAsJsonAsync("/api/generate", payload, ct);
            httpRes.EnsureSuccessStatusCode();
            using var doc = await JsonDocument.ParseAsync(await httpRes.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

            if (doc.RootElement.TryGetProperty("output", out var output) && output.ValueKind == JsonValueKind.Array && output.GetArrayLength() > 0)
            {
                var first = output[0];
                if (first.TryGetProperty("content", out var content))
                    return new ModelResponse { Text = content.GetString() ?? string.Empty };
            }

            if (doc.RootElement.TryGetProperty("response", out var textProp))
                return new ModelResponse { Text = textProp.GetString() ?? string.Empty };

            return new ModelResponse { Text = doc.RootElement.ToString() };
        }
    }
}
