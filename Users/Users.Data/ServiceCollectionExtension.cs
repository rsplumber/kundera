using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Data.UserGroups;
using Users.Data.Users;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Data;

internal static class ServiceCollectionExtension
{
    public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
    }
}