using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using RoleManagement.Data.Redis;

namespace RoleManagement.Builder;

public static class ApplicationBuilderExtension
{
    public static void ConfigureRoleManagement(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.ConfigureDataLayer(configuration);
    }
}