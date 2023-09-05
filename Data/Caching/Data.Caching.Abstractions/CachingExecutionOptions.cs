namespace Data.Caching.Abstractions;

public sealed class CachingExecutionOptions
{
    public required IServiceProvider ServiceProvider { get; init; } = default!;
}