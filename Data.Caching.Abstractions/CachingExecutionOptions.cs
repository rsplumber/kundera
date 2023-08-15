namespace Data.Caching.Abstractions;

public sealed class CachingExecutionOptions
{
    public IServiceProvider ServiceProvider { get; init; } = default!;
}