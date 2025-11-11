using Microsoft.Extensions.Logging;
using MyAi.Core.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyAi.Infrastructure.Services
{
    public class LocalModelClient : IModelClient
    {
        private readonly HttpClient _http;
 
        private readonly ILogger<LocalModelClient> _logger;
        public string Name => "phi4";

        public LocalModelClient(HttpClient httpClient, ILogger<LocalModelClient> logger)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    
            _logger = logger;
        }

        public async Task<ModelResponse> GenerateAsync(string prompt, ModelRequestOptions options, CancellationToken ct = default)
        {
            var sw = Stopwatch.StartNew();
            var est = TokenEstimator.Estimate(prompt);
            var payload = new { model = Name, prompt, temperature = options.Temperature, max_tokens = options.MaxTokens };
            var json = JsonSerializer.Serialize(payload);
            var resp = await _http.PostAsync("/api/generate", new StringContent(json, Encoding.UTF8, "application/json"), ct);
            resp.EnsureSuccessStatusCode();

            var sb = new StringBuilder();

            using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    using var doc = JsonDocument.Parse(line);
                    if (doc.RootElement.TryGetProperty("response", out var res))
                        sb.Append(res.GetString());
                }
                catch (JsonException)
                {
                    // skip malformed or partial lines
                }
            }

            _logger.LogInformation("Provider:{Provider}, LatencyMs:{Ms}, TokenEst:{Est}", Name, sw.ElapsedMilliseconds, est);

            return new ModelResponse { Text = sb.ToString() };
        }

    }
}
