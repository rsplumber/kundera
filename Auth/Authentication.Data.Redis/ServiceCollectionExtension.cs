using Auth.Domain.Credentials;
using Kite.Serializer.Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Authentication.Data.Redis;

internal static class ServiceCollectionExtension
{
    public static void AddAuthenticationDataRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer();
        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Authentication")));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}