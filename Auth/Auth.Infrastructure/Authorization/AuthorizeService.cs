using System.Net;
using Auth.Application.Authorization;
using Auth.Domain.Sessions;
using Domain.Roles;
using Domain.Scopes;
using Domain.Services;
using Domain.UserGroups;
using Domain.Users;
using Domain.Users.Types;

namespace Authentication.Infrastructure.Authorization;

internal sealed class AuthorizeService : IAuthorizeService
{
    private readonly ISessionManagement _sessionManagement;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScopeRepository _scopeRepository;
    private readonly IUserGroupRepository _userGroupRepository;

    public AuthorizeService(ISessionManagement sessionManagement,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IScopeRepository scopeRepository,
        IUserGroupRepository userGroupRepository)
    {
        _sessionManagement = sessionManagement;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _scopeRepository = scopeRepository;
        _userGroupRepository = userGroupRepository;
    }

    public async ValueTask AuthorizeAsync(Token token,
        string action,
        string scope,
        string? service,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(token, ipAddress ?? IPAddress.None, cancellationToken);
        if (session is null)
        {
            throw new UnAuthorizedException();
        }

        if (TokenExpired() || InvalidScope())
        {
            throw new UnAuthorizedException();
        }

        var user = await _userRepository.FindAsync(UserId.From(session.UserId), cancellationToken);
        if (user is null || UserIsNotActive())
        {
            throw new UnAuthorizedException();
        }

        var sessionScope = await _scopeRepository.FindAsync(ScopeId.From(session.Scope), cancellationToken);
        if (sessionScope is null || UserHasNotScopeRole() || InvalidService())
        {
            throw new UnAuthorizedException();
        }

        var permissions = await FetchPermissionsAsync();

        if (!permissions.Any(action.ToLower().Equals))
        {
            throw new UnAuthorizedException();
        }

        bool TokenExpired() => DateTime.UtcNow >= session.ExpiresAt;

        bool InvalidScope() => !session.Scope.Equals(scope);

        bool InvalidService() => service is null || sessionScope.Services.All(id => id != ServiceId.From(service));

        bool UserIsNotActive() => user.Status != UserStatus.Active;

        bool UserHasNotScopeRole() => !user.Roles.Any(id => sessionScope.Has(id));

        async ValueTask<IEnumerable<string>> FetchPermissionsAsync()
        {
            var userRoles = await _roleRepository.FindAsync(user.Roles.ToArray(), cancellationToken);
            var userGroupRoles = await FetchUserGroupsRolesAsync();
            var roles = userRoles.ToList();
            roles.AddRange(userGroupRoles);
            return roles.SelectMany(role => role.Permissions.Select(id => id.Value));
        }

        async ValueTask<IEnumerable<Role>> FetchUserGroupsRolesAsync()
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