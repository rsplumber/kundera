using Microsoft.Extensions.DependencyInjection;

namespace Data.Caching.Abstractions;

public sealed class CachingOptions
{
    public IServiceCollection Services { get; init; } = default!;
}