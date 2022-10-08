using Auth.Domain.Sessions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Tes.Serializer.Microsoft;

namespace Authorization.Data.Redis;

internal static class ServiceCollectionExtension
{
    public static void AddAuthorizationDataRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Authorization")));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}