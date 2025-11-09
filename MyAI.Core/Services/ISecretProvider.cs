namespace MyAi.Core.Services
{
    public interface ISecretProvider
    {
        string? GetSecret(string key);
    }
}
