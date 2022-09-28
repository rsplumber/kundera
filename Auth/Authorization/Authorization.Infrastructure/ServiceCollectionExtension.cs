using Authorization.Application;
using Authorization.Domain;
using Authorization.Infrastructure.Data;
using Kundera.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.Serializer.Microsoft;

namespace Authorization.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftSerializer(configuration);

        services.AddScoped<ISessionRepository, SessionRepository>();

        services.AddScoped<IAuthorizeService, AuthorizeService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<ISessionManagement, SessionManagement>();
    }
}