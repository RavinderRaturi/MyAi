using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MyAi.Core.Services;
using Scriban;

namespace MyAi.Infrastructure.Services
{
    public class ScribanPromptTemplateService : IPromptTemplateService
    {
        private readonly string _promptRoot;
        private readonly ConcurrentDictionary<string, Template> _cache = new();

        public ScribanPromptTemplateService(string promptRoot)
        {
            _promptRoot = promptRoot ?? throw new ArgumentNullException(nameof(promptRoot));
        }

        public async Task<string> RenderAsync(string templateName, IDictionary<string, object> model)
        {
            if (string.IsNullOrWhiteSpace(templateName)) throw new ArgumentException("templateName is required", nameof(templateName));

            var path = Path.Combine(_promptRoot, templateName);
            if (!File.Exists(path)) throw new FileNotFoundException($"Template not found: {path}");

            // Get or parse template once
            var template = _cache.GetOrAdd(path, p =>
            {
                var text = File.ReadAllText(p);
                var parsed = Template.Parse(text);
                if (parsed.HasErrors)
                {
                    // Provide diagnostic info to developers
                    throw new InvalidDataException($"Scriban template parse errors in '{p}': {string.Join("; ", parsed.Messages)}");
                }
                return parsed;
            });

            // Build a Scriban script object from model dictionary
            var scriptObject = new Scriban.Runtime.ScriptObject();
            if (model != null)
            {
                foreach (var kv in model) scriptObject.Add(kv.Key, kv.Value);
            }

            var context = new Scriban.TemplateContext();
            context.PushGlobal(scriptObject);

            // Render synchronously but keep Task signature for async compatibility
            var result = await Task.FromResult(template.Render(context));
            return result;
        }
    }
}
