using Managements.Data.Auth.Credentials;
using Managements.Data.Auth.Sessions;
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
    public static void UseData(this IApplicationBuilder app)
    {
        try
        {
            var dbProvider = app.ApplicationServices.GetRequiredService<RedisConnectionProvider>();
            new List<Type>
            {
                typeof(GroupDataModel),
                typeof(UserDataModel),
                typeof(RoleDataModel),
                typeof(PermissionDataModel),
                typeof(ScopeDataModel),
                typeof(ServiceDataModel),
                typeof(CredentialDataModel),
                typeof(SessionDataModel),
                typeof(AuthorizeCacheDataModel),
            }.ForEach(type =>
            {
                if (dbProvider.Connection.GetIndexInfo(type) is null)
                {
                    dbProvider.Connection.CreateIndex(type);
                }
            });
        }
        catch (Exception)
        {
            // ignored
        }
    }
}