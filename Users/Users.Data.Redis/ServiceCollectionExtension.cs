using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Users.Data.Redis.UserGroups;
using Users.Data.Redis.Users;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Data.Redis;

internal static class ServiceCollectionExtension
{
    public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddSingleton(new RedisConnectionProvider(configuration.GetConnectionString("Users")));
    }
}