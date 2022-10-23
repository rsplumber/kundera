using System.Net;
using Auth.Core;
using Auth.Core.Services;
using Managements.Domain;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.Services.Types;
using Managements.Domain.UserGroups;
using Managements.Domain.Users;
using Managements.Domain.Users.Types;

namespace Auth.Services;

internal sealed class AuthorizeService : IAuthorizeService
{
    private readonly ISessionManagement _sessionManagement;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AuthorizeService(ISessionManagement sessionManagement,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IScopeRepository scopeRepository,
        IUserGroupRepository userGroupRepository,
        IServiceRepository serviceRepository,
        IPermissionRepository permissionRepository)
    {
        _sessionManagement = sessionManagement;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _scopeRepository = scopeRepository;
        _userGroupRepository = userGroupRepository;
        _serviceRepository = serviceRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<Guid> AuthorizeAsync(Token token,
        string action,
        string serviceSecret,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(token, ipAddress ?? IPAddress.None, cancellationToken);
        if (session is null)
        {
            throw new UnAuthorizedException();
        }

        if (TokenExpired())
        {
            throw new UnAuthorizedException();
        }

        var user = await _userRepository.FindAsync(UserId.From(session.UserId), cancellationToken);
        if (InvalidUser())
        {
            throw new UnAuthorizedException();
        }

        var userRoles = await user!.RolesWithParentRolesAsync(_userGroupRepository, _roleRepository);

        var sessionScope = await _scopeRepository.FindAsync(ScopeId.From(session.ScopeId), cancellationToken);
        if (sessionScope is null || UserHasNotScopeRole())
        {
            throw new UnAuthorizedException();
        }

        var service = await _serviceRepository.FindAsync(ServiceSecret.From(serviceSecret), cancellationToken);
        if (!userRoles.Any(role => role.Name == EntityBaseValues.SuperAdminRole) && InvalidService())
        {
            throw new UnAuthorizedException();
        }


        var permissionIds = userRoles.SelectMany(role => role.Permissions.Select(id => id));
        var permissions = await _permissionRepository.FindAsync(permissionIds, cancellationToken);
        if (!permissions.Any(permission => permission.Name.Value.Equals(action.ToLower())))
        {
            throw new UnAuthorizedException();
        }

        return user.Id.Value;

        bool TokenExpired() => DateTime.UtcNow >= session.ExpiresAt;

        bool InvalidService()
        {
            return service is null || sessionScope.Services.All(id => id != service.Id);
        }

        bool InvalidUser() => user is null || user.Status != UserStatus.Active;

        bool UserHasNotScopeRole() => !userRoles.Any(role => sessionScope.Has(role.Id));
    }
}