using System.Reflection;
using Auth.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Auth.Data;

internal static class ServiceCollectionExtension
{
    public static Assembly AddAuthData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Auth")));
        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return Assembly.GetExecutingAssembly();
    }
}