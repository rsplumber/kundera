using Authentication.Application;
using Authentication.Domain;
using Authentication.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Tes.Serializer.Microsoft;

namespace Authentication.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);

        services.AddScoped<ICredentialRepository, CredentialRepository>();

        services.AddScoped<IAuthenticateService, AuthenticateService>();

        services.AddScoped<ICredentialService, CredentialService>();

        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Authentication")));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}