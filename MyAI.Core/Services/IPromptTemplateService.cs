using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MyAi.Core.Services;
public interface IPromptTemplateService
{
    Task<string> RenderAsync(string templateName, IDictionary<string, object> model);
}