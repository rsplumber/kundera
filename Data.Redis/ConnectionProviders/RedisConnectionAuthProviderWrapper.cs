using Redis.OM;
using StackExchange.Redis;

namespace Managements.Data.ConnectionProviders;

internal class RedisConnectionAuthProviderWrapper : RedisConnectionProvider
{
    public RedisConnectionAuthProviderWrapper(string connectionString) : base(connectionString)
    {
    }

    public RedisConnectionAuthProviderWrapper(RedisConnectionConfiguration connectionConfig) : base(connectionConfig)
    {
    }

    public RedisConnectionAuthProviderWrapper(ConfigurationOptions configurationOptions) : base(configurationOptions)
    {
    }

    public RedisConnectionAuthProviderWrapper(IConnectionMultiplexer connectionMultiplexer) : base(connectionMultiplexer)
    {
    }
}