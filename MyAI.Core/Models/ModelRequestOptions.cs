public sealed class ModelRequestOptions
{
    public double Temperature { get; init; } = 0.7;
    public int MaxTokens { get; init; } = 512;
    public bool Stream { get; init; } = false;
}
