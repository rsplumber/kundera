using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.Web.Validators;

namespace Authorization.Web.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddAuthorizationWeb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddRequestValidators(configuration);
    }
}