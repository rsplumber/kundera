using Core.Auth.Authorizations;
using Core.Auth.Credentials;
using Core.Auth.Sessions;
using Core.Groups;
using Core.Permissions;
using Core.Roles;
using Core.Scopes;
using Core.Services;
using Core.Users;
using Data.Abstractions;
using Data.Auth;
using Data.Auth.Credentials;
using Data.Auth.Sessions;
using Data.Groups;
using Data.Permissions;
using Data.Roles;
using Data.Scopes;
using Data.Services;
using Data.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Data;

public static class DataOptionsExtension
{
    public static void UseEntityFramework(this DataOptions dataOptions, Action<DbContextOptionsBuilder> optionsAction)
    {
        dataOptions.Services.AddDbContext<AppDbContext>(optionsAction);
        dataOptions.Services.TryAddScoped<IUserRepository, UserRepository>();
        dataOptions.Services.TryAddScoped<IGroupRepository, GroupRepository>();
        dataOptions.Services.TryAddScoped<IServiceRepository, ServiceRepository>();
        dataOptions.Services.TryAddScoped<IScopeRepository, ScopeRepository>();
        dataOptions.Services.TryAddScoped<IRoleRepository, RoleRepository>();
        dataOptions.Services.TryAddScoped<IPermissionRepository, PermissionRepository>();
        dataOptions.Services.TryAddScoped<ICredentialRepository, CredentialRepository>();
        dataOptions.Services.TryAddScoped<IAuthenticationActivityRepository, AuthenticationActivityRepository>();
        dataOptions.Services.TryAddScoped<ISessionRepository, SessionRepository>();
        dataOptions.Services.TryAddScoped<IAuthorizationActivityRepository, AuthorizationActivityRepository>();
        dataOptions.Services.TryAddScoped<IExpiredAuthenticationActivityService, ExpiredAuthenticationActivityService>();
        dataOptions.Services.TryAddScoped<IExpiredCredentialsService, ExpiredCredentialsService>();
        dataOptions.Services.TryAddScoped<IExpiredAuthorizationActivityService, ExpiredAuthorizationActivityService>();
        dataOptions.Services.TryAddScoped<IExpiredSessionsService, ExpiredSessionsService>();
        dataOptions.Services.AddScoped<IAuthorizeDataProvider, AuthorizeDataProvider>();
    }
}