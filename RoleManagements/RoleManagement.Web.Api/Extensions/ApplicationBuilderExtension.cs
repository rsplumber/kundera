using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Tes.Common.Identity.Web.Authorization;

namespace RoleManagement.Web.Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static void ConfigureRoleManagementWeb(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseRouting();
        app.UseFakeWebAuthorization(configuration);
    }
}