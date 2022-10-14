using Auth.Domain.Sessions;
using Kite.Serializer.Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Authorization.Data.Redis;

internal static class ServiceCollectionExtension
{
    public static void AddAuthorizationDataRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Authorization")));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}