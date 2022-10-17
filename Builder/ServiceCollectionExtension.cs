using Auth.Builder;
using Kite.Cache.InMemory;
using Kite.Serializer.Microsoft;
using Kite.Tokens.JWT;
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
        services.AddTokensJwt(configuration);

        services.AddAuth(configuration);
        services.AddManagements(configuration);
    }
}