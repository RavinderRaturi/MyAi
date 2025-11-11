using MyAi.Core.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MyAi.Core.Services.ISecretProvider, MyAi.Infrastructure.Services.EnvVarSecretProvider>();
builder.Services.AddHttpClient<MyAi.Core.Services.IModelClient, MyAi.Infrastructure.Services.LocalModelClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["OLLAMA_BASE_URL"] ?? "http://localhost:11434");
});

// determine prompts folder path relative to content root (MyAi.Api)
var promptsRoot = Path.Combine(builder.Environment.ContentRootPath, "prompts");

// register the prompt service as singleton so parsed templates are cached app-wide
builder.Services.AddSingleton<MyAi.Core.Services.IPromptTemplateService>(_ => new MyAi.Infrastructure.Services.ScribanPromptTemplateService(promptsRoot));




var app = builder.Build();
app.MapGet("/", () => "ok");


//Just for debugging prompt rendering and testing purposes. not for production use.
//app.MapGet("/debug/prompt", async (IPromptTemplateService prompts) =>
//{
//    var model = new Dictionary<string, object>
//    {
//        ["user_instructions"] = "Summarize: Large language models are powerful but costly."
//    };

//    var rendered = await prompts.RenderAsync("chat.prompt.v1.md", model);
//    Console.WriteLine("Rendered prompt:\n" + rendered);
//    return Results.Text(rendered);
//});
app.Run();
