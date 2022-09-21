using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.Web.Validators;

namespace Web.Api;

public static class ServiceCollectionExtension
{
    public static void AddKunderaWeb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddRequestValidators(configuration);
    }
}