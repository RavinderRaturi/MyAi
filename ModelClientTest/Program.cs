using System;
using Microsoft.Extensions.DependencyInjection;
using MyAi.Core.Services;
using MyAi.Infrastructure.Services;

var services = new ServiceCollection();

services.AddHttpClient<IModelClient, LocalModelClient>(c =>
{
    var baseUrl = Environment.GetEnvironmentVariable("OLLAMA_BASE_URL") ?? "http://localhost:11434";
    c.BaseAddress = new Uri(baseUrl);
});

// If you want to use mock for quick tests set OLLAMA_DISABLE=1 in environment
var disableLocal = Environment.GetEnvironmentVariable("OLLAMA_DISABLE");
if (!string.IsNullOrEmpty(disableLocal) && disableLocal == "1")
{
    services.AddSingleton<IModelClient, MockModelClient>();
}

var sp = services.BuildServiceProvider();
var client = sp.GetRequiredService<IModelClient>();

Console.WriteLine("Sending prompt...");
var result = await client.GenerateAsync("Say hello in one sentence.");
Console.WriteLine("Model output:");
Console.WriteLine(result.Text);
