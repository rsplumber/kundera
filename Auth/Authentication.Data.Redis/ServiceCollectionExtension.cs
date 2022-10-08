using Auth.Domain.Credentials;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Tes.Serializer.Microsoft;

namespace Authentication.Data.Redis;

internal static class ServiceCollectionExtension
{
    public static void AddAuthenticationDataRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);
        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Authentication")));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}