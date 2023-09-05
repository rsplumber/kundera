using KunderaNet.Authorization;
using KunderaNet.Services.Authorization.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KunderaNet.FastEndpoints.Authorization;

public static class AuthenticationBuilderExtension
{
    public static void AddKundera(this AuthenticationBuilder builder, IConfiguration configuration, Action<AuthorizationServiceConfiguration> serviceConfiguration)
    {
        var serviceSecret = configuration.GetSection("Kundera:ServiceSecret").Value;
        if (serviceSecret is null)
        {
            throw new ArgumentException("Enter Kundera:ServiceSecret in appsettings.json");
        }

        serviceConfiguration(new AuthorizationServiceConfiguration(builder.Services));
        KunderaAuthorizationSettings.ServiceSecret = serviceSecret;
        builder.Services.AddHttpContextAccessor();
        builder.AddScheme<KunderaAuthenticationOption, KunderaAuthenticationHandler>(KunderaDefaults.Scheme, scheme => scheme.Validate());
        builder.Services.TryAddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.TryAddScoped<ICurrentSessionService, CurrentSessionService>();
        builder.Services.AddScoped<IPolicyEvaluator, KunderaPolicyEvaluator>();
    }
}