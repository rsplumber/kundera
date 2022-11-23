using Auth.Application.BackgroundJobs;
using Auth.Application.Data;
using Auth.Application.Services;
using Auth.Core;
using Auth.Core.Entities;
using Auth.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

internal static class ServiceCollectionExtension
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthData(configuration);

        services.AddScoped<ISessionFactory, SessionFactory>();
        services.AddScoped<ICredentialFactory, CredentialFactory>();

        services.AddScoped<IAuthorizeService, AuthorizeService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<ISessionManagement, SessionManagement>();

        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<ICredentialService, CredentialService>();
        services.Configure<SessionOptions>(configuration.GetSection("Sessions"));

        services.AddHostedService<RemoveExpiredCredentialsJob>();
        services.AddHostedService<RemoveExpiredSessionsJob>();
    }
}