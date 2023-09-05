using Microsoft.Extensions.DependencyInjection;

namespace Data.Caching.Abstractions;

public sealed class CachingOptions
{
    public required IServiceCollection Services { get; init; } = default!;
}