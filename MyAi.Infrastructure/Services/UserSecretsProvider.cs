using Microsoft.Extensions.Configuration;
using MyAi.Core.Services;

namespace MyAi.Infrastructure.Services
{
    public class UserSecretsProvider : ISecretProvider
    {
        private readonly IConfiguration _config;
        public UserSecretsProvider(IConfiguration config) => _config = config;
        public string? GetSecret(string key) => _config[key];
    }
}
