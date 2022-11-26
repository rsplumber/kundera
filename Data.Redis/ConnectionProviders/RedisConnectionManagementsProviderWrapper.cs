using Redis.OM;
using StackExchange.Redis;

namespace Managements.Data.ConnectionProviders;

internal class RedisConnectionManagementsProviderWrapper : RedisConnectionProvider
{
    public RedisConnectionManagementsProviderWrapper(string connectionString) : base(connectionString)
    {
    }

    public RedisConnectionManagementsProviderWrapper(RedisConnectionConfiguration connectionConfig) : base(connectionConfig)
    {
    }

    public RedisConnectionManagementsProviderWrapper(ConfigurationOptions configurationOptions) : base(configurationOptions)
    {
    }

    public RedisConnectionManagementsProviderWrapper(IConnectionMultiplexer connectionMultiplexer) : base(connectionMultiplexer)
    {
    }
}