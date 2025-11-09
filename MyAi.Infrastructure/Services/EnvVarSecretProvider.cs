using MyAi.Core.Services;
using System;

namespace MyAi.Infrastructure.Services
{
    public class EnvVarSecretProvider : ISecretProvider
    {
        public string? GetSecret(string key) => Environment.GetEnvironmentVariable(key);
    }
}
