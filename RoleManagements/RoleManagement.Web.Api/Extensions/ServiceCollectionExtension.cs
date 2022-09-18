using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tes.Common.Identity.Web.Authorization;
using Tes.Web.Validators;

namespace RoleManagement.Web.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddRoleManagementWeb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddRequestValidators(configuration);
        services.AddWebAuthorization(configuration);
    }
}