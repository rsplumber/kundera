using System.Reflection;
using Managements.Data.Groups;
using Managements.Data.Permissions;
using Managements.Data.Roles;
using Managements.Data.Scopes;
using Managements.Data.Services;
using Managements.Data.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Managements.Data;

internal static class ApplicationBuilderExtension
{
    public static Assembly ConfigureManagementsData(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()
            ?.CreateScope();

        if (serviceScope is null) return Assembly.GetExecutingAssembly();

        try
        {
            var dbProvider = serviceScope.ServiceProvider.GetRequiredService<RedisConnectionManagementsProviderWrapper>();
            new List<Type>
            {
                typeof(GroupDataModel),
                typeof(UserDataModel),
                typeof(RoleDataModel),
                typeof(PermissionDataModel),
                typeof(ScopeDataModel),
                typeof(ServiceDataModel)
            }.ForEach(type =>
            {
                if (dbProvider.Connection.GetIndexInfo(type) is null)
                {
                    dbProvider.Connection.CreateIndex(type);
                }
            });
        }
        catch (Exception e)
        {
            // ignored
        }

        return Assembly.GetExecutingAssembly();
    }
}