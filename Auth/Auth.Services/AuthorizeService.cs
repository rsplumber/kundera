using System.Net;
using Auth.Core;
using Auth.Core.Services;
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

        var userRoles = await FetchAllRoles();

        var sessionScope = await _scopeRepository.FindAsync(ScopeId.From(session.ScopeId), cancellationToken);
        if (sessionScope is null || UserHasNotScopeRole())
        {
            throw new UnAuthorizedException();
        }

        var service = await _serviceRepository.FindAsync(ServiceSecret.From(serviceSecret), cancellationToken);
        if (InvalidService())
        {
            throw new UnAuthorizedException();
        }


        var permissionIds = userRoles.SelectMany(role => role.Permissions.Select(id => id));
        var permissions = await _permissionRepository.FindAsync(permissionIds, cancellationToken);
        if (!permissions.Any(permission => permission.Name.Value.Equals(action.ToLower())))
        {
            throw new UnAuthorizedException();
        }

        return user!.Id.Value;

        bool TokenExpired() => DateTime.UtcNow >= session.ExpiresAt;

        bool InvalidService()
        {
            return service is null || sessionScope.Services.All(id => id != service.Id);
        }

        bool InvalidUser() => user is null || user.Status != UserStatus.Active;

        async Task<IEnumerable<Role>> FetchAllRoles()
        {
            var userRoles = await _roleRepository.FindAsync(user.Roles.ToArray(), cancellationToken);
            var userGroupRoles = await FetchUserGroupsRolesAsync();
            var roles = userRoles.ToList();
            roles.AddRange(userGroupRoles);
            return roles;
        }

        bool UserHasNotScopeRole() => !userRoles.Any(role => sessionScope.Has(role.Id));

        async Task<IEnumerable<Role>> FetchUserGroupsRolesAsync()
        {
            var groups = await _userGroupRepository.FindAsync(user.UserGroups.ToArray(), cancellationToken);
            var groupList = groups.ToHashSet();
            foreach (var userGroup in groups)
            {
                FetchOrganizationParents(userGroup);
            }

            var roleIds = groupList.SelectMany(group => group.Roles.Select(id => id)).ToArray();

            return await _roleRepository.FindAsync(roleIds, cancellationToken);

            void FetchOrganizationParents(UserGroup userGroup)
            {
                while (true)
                {
                    if (userGroup.Parent is not null)
                    {
                        var org = _userGroupRepository.FindAsync(userGroup.Parent, cancellationToken).Result;
                        if (org is null) continue;
                        userGroup = org;
                        groupList.Add(org);
                        continue;
                    }

                    break;
                }
            }
        }
    }
}