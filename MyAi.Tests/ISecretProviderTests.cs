using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using MyAi.Infrastructure.Services;
using System.Collections.Generic;
using System;

namespace MyAi.Tests
{
    public class ISecretProviderTests
    {
        [Test]
        public void UserSecretsProvider_ReturnsValueFromConfiguration()
        {
            var ini = new Dictionary<string, string?> { ["SomeKey"] = "secret-value" };
            var config = new ConfigurationBuilder().AddInMemoryCollection(ini).Build();
            var p = new UserSecretsProvider(config);
            Assert.AreEqual("secret-value", p.GetSecret("SomeKey"));
        }

        [Test]
        public void EnvVarSecretProvider_ReturnsEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("TEST_KEY", "env-value");
            var p = new EnvVarSecretProvider();
            Assert.AreEqual("env-value", p.GetSecret("TEST_KEY"));
        }
    }
}
