using System;
using Microsoft.Extensions.DependencyInjection;
using MyAi.Core.Services;
using MyAi.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

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

var clientOptions = new ModelRequestOptions
{
    Temperature = 0.7,
    MaxTokens = 100,
};

var result = await client.GenerateAsync("Just greet me in 3 language other than English. also name the language. I need only one example.", clientOptions);
Console.WriteLine("Model output:");
Console.WriteLine(result.Text);
