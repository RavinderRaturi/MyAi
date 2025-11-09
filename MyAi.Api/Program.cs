var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MyAi.Core.Services.ISecretProvider, MyAi.Infrastructure.Services.EnvVarSecretProvider>();
builder.Services.AddHttpClient<MyAi.Core.Services.IModelClient, MyAi.Infrastructure.Services.LocalModelClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["OLLAMA_BASE_URL"] ?? "http://localhost:11434");
});
var app = builder.Build();
app.MapGet("/", () => "ok");
app.Run();
