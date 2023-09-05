using Data.Abstractions;
using Data.Auth.Credentials;
using Data.Auth.Sessions;
using Data.Groups;
using Data.Permissions;
using Data.Roles;
using Data.Scopes;
using Data.Services;
using Data.Users;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;

namespace Data;

public static class DataExecutionOptionsExtension
{
    public static void UseRedis(this DataExecutionOptions dataExecutionOptions)
    {
        try
        {
            var dbProvider = dataExecutionOptions.ServiceProvider.GetRequiredService<RedisConnectionProvider>();
            new List<Type>
            {
                typeof(GroupDataModel),
                typeof(UserDataModel),
                typeof(RoleDataModel),
                typeof(PermissionDataModel),
                typeof(ScopeDataModel),
                typeof(ServiceDataModel),
                typeof(CredentialDataModel),
                typeof(SessionDataModel)
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