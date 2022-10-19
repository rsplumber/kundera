using Auth.Builder;
using Kite.Cache.InMemory;
using Kite.Hashing.HMAC;
using Kite.Serializer.Microsoft;
using Managements.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Builder;

public static class ServiceCollectionExtension
{
    public static void AddKundera(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer();
        services.AddCacheInMemory();
        services.AddHashHmac();

        services.AddAuth(configuration);
        services.AddManagements(configuration);
    }
}