using System.Net;
using Auth.Core;
using Auth.Core.Services;
using Auth.Services.Events;
using Kite.Events;
using Managements.Domain;
using Managements.Domain.Groups;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.Services.Types;
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
    private readonly IGroupRepository _groupRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IEventBus _eventBus;

    public AuthorizeService(ISessionManagement sessionManagement,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IScopeRepository scopeRepository,
        IGroupRepository groupRepository,
        IServiceRepository serviceRepository,
        IPermissionRepository permissionRepository,
        IEventBus eventBus)
    {
        _sessionManagement = sessionManagement;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _scopeRepository = scopeRepository;
        _groupRepository = groupRepository;
        _serviceRepository = serviceRepository;
        _permissionRepository = permissionRepository;
        _eventBus = eventBus;
    }

    public async Task<Guid> AuthorizeAsync(Token token,
        string action,
        string serviceSecret,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(token, cancellationToken);
        if (session is null)
        {
            throw new UnAuthorizedException();
        }

        if (Expired())
        {
            throw new SessionExpiredException();
        }

        var user = await _userRepository.FindAsync(UserId.From(session.UserId), cancellationToken);
        if (InvalidUser())
        {
            throw new UnAuthorizedException();
        }

        var roles = await RolesAsync(user!, cancellationToken);

        var sessionScope = await _scopeRepository.FindAsync(ScopeId.From(session.ScopeId), cancellationToken);
        if (InvalidSessionScope())
        {
            throw new UnAuthorizedException();
        }

        var service = await _serviceRepository.FindAsync(ServiceSecret.From(serviceSecret), cancellationToken);
        if (InvalidService())
        {
            if (service is null || service.Name != EntityBaseValues.KunderaServiceName)
            {
                throw new UnAuthorizedException();
            }
        }

        var permissionIds = roles.DistinctBy(role => role.Id)
            .SelectMany(role => role.Permissions)
            .Distinct();
        var permissions = await _permissionRepository.FindAsync(permissionIds, cancellationToken);
        if (InvalidPermission())
        {
            throw new UnAuthorizedException();
        }

        await _eventBus.PublishAsync(new OnAuthorizeEvent(user!.Id.Value,
            sessionScope!.Id.Value,
            service!.Id.Value,
            action,
            ipAddress ?? IPAddress.None), cancellationToken);

        return user.Id.Value;

        bool Expired() => DateTime.UtcNow >= session.ExpiresAt;

        bool InvalidUser() => user is null || user.Status != UserStatus.Active;

        bool InvalidSessionScope() => sessionScope is null || UserHasNotScopeRole();

        bool UserHasNotScopeRole() => !roles.Any(role => sessionScope.Has(role.Id));

        bool InvalidService() => service is null || sessionScope!.Services.All(id => id != service.Id);

        bool InvalidPermission() => permissions.All(permission => permission.Name != action.ToLower());
    }

    private async Task<IReadOnlyCollection<Role>> RolesAsync(User user, CancellationToken cancellationToken)
    {
        var userGroups = await UserGroupsAsync();
        var allRoles = new List<Role>();
        var userRoles = await _roleRepository.FindAsync(user.Roles, cancellationToken);
        var groupRoles = await _roleRepository.FindAsync(userGroups.SelectMany(group => group.Roles), cancellationToken);
        allRoles.AddRange(userRoles);
        allRoles.AddRange(groupRoles);
        return allRoles;

        async Task<IReadOnlyCollection<Group>> UserGroupsAsync()
        {
            var groups = new List<Group>();
            var currentUserGroups = await _groupRepository.FindAsync(user.Groups, cancellationToken);
            groups.AddRange(currentUserGroups);
            foreach (var userGroupId in user.Groups)
            {
                var children = await _groupRepository.FindChildrenAsync(userGroupId, cancellationToken);
                groups.AddRange(children);
            }

            return groups;
        }
    }
}