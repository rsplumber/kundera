using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.Web.Validators;

namespace Authentication.Web.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddAuthenticationWeb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddRequestValidators(configuration);
    }
}