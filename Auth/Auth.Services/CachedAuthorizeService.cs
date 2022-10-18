using System.Net;
using Auth.Core;
using Auth.Core.Services;
using Kite.Cache;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.UserGroups;
using Managements.Domain.Users;
using Managements.Domain.Users.Types;

namespace Auth.Services;

internal sealed class CachedAuthorizeService : IAuthorizeService
{
    private const int HalfDayCacheExpireTime = 60 * 60 * 12;
    private readonly ISessionManagement _sessionManagement;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScopeRepository _scopeRepository;
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly ICacheManager _cacheManager;

    public CachedAuthorizeService(ISessionManagement sessionManagement,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IScopeRepository scopeRepository,
        IUserGroupRepository userGroupRepository,
        ICacheManager cacheManager)
    {
        _sessionManagement = sessionManagement;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _scopeRepository = scopeRepository;
        _userGroupRepository = userGroupRepository;
        _cacheManager = cacheManager;
    }

    public async Task<Guid> AuthorizeAsync(Token token,
        string action,
        string scope,
        string? service,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var session = await FetchSessionAsync(token, scope, service, ipAddress, cancellationToken);
        if (!session.Permissions.Any(action.ToLower().Equals))
        {
            throw new UnAuthorizedException();
        }

        return session.UserId;
    }

    private async Task<SessionCache> FetchSessionAsync(Token token,
        string scope,
        string? service,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKey.From<SessionCache>(token.Value);
        var sessionCache = await _cacheManager.GetAsync<SessionCache>(cacheKey, cancellationToken);
        if (sessionCache is not null) return sessionCache;

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

        var allRoles = await FetchAllRoles();

        var sessionScope = await _scopeRepository.FindAsync(ScopeId.From(session.Scope), cancellationToken);
        if (sessionScope is null || UserHasNotScopeRole() || InvalidService())
        {
            throw new UnAuthorizedException();
        }

        var permissions = allRoles.SelectMany(role => role.Permissions.Select(id => id.Value))
            .ToList();
        var cache = new SessionCache(user.Id.Value, permissions);
        await _cacheManager.SetAsync(cacheKey, cache, HalfDayCacheExpireTime, cancellationToken);
        return cache;

        bool TokenExpired() => DateTime.UtcNow >= session.ExpiresAt;

        bool InvalidScope() => !session.Scope.Equals(scope);

        bool InvalidService()
        {
            return sessionScope.Services.All(id => id != ServiceId.From(service ?? "all"));
        }

        bool UserIsNotActive() => user.Status != UserStatus.Active;

        async Task<IEnumerable<Role>> FetchAllRoles()
        {
            var userRoles = await _roleRepository.FindAsync(user.Roles.ToArray(), cancellationToken);
            var userGroupRoles = await FetchUserGroupsRolesAsync();
            var roles = userRoles.ToList();
            roles.AddRange(userGroupRoles);
            return roles;
        }

        bool UserHasNotScopeRole() => !allRoles.Any(role => sessionScope.Has(role.Id));

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

public sealed record SessionCache(Guid UserId, List<string> Permissions);