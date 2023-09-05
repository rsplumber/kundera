using Data.Abstractions;

namespace Data.Caching.Abstractions;

public static class DataExecutionOptionsExtension
{
    public static void UseCaching(this DataExecutionOptions dataExecutionOptions, Action<CachingExecutionOptions>? options) => options?.Invoke(new CachingExecutionOptions
    {
        ServiceProvider = dataExecutionOptions.ServiceProvider
    });
}