using Core.Auth.Authorizations;
using Core.Auth.Sessions;
using Core.Roles;
using Core.Services;
using Core.Users;
using Data.Caching.Redis.CacheManagements;

namespace Data.Caching.Redis;

internal sealed class AuthorizeDataProvider : IAuthorizeDataProvider
{
    private readonly IAuthorizeDataProvider _authorizeDataProvider;
    private readonly SessionCacheManagement _sessionCacheManagement;
    private readonly UserRoleCacheManagement _userRoleCacheManagement;
    private readonly ServiceCacheManagement _serviceCacheManagement;

    public AuthorizeDataProvider(IAuthorizeDataProvider authorizeDataProvider,
        SessionCacheManagement sessionCacheManagement,
        ServiceCacheManagement serviceCacheManagement,
        UserRoleCacheManagement userRoleCacheManagement)
    {
        _authorizeDataProvider = authorizeDataProvider;
        _sessionCacheManagement = sessionCacheManagement;
        _serviceCacheManagement = serviceCacheManagement;
        _userRoleCacheManagement = userRoleCacheManagement;
    }


    public async Task<Session?> FindSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        var cachedSession = await _sessionCacheManagement.GetAsync(sessionToken, cancellationToken);
        if (cachedSession is not null)
        {
            return cachedSession;
        }

        var session = await _authorizeDataProvider.FindSessionAsync(sessionToken, cancellationToken);
        if (session is null) return null;
        await _sessionCacheManagement.SetAsync(session, cancellationToken);
        return session;
    }

    public async Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        var cachedRoles = await _userRoleCacheManagement.GetAsync(user.Id, cancellationToken);
        if (cachedRoles is not null)
        {
            return cachedRoles;
        }

        var roles = await _authorizeDataProvider.UserRolesAsync(user, cancellationToken);
        await _userRoleCacheManagement.SetAsync(user.Id, roles, cancellationToken);
        return roles;
    }

    public async Task<Service?> FindServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        var cachedService = await _serviceCacheManagement.GetAsync(serviceSecret, cancellationToken);
        if (cachedService is not null)
        {
            return cachedService;
        }

        var service = await _authorizeDataProvider.FindServiceAsync(serviceSecret, cancellationToken);
        if (service is null) return null;
        await _serviceCacheManagement.SetAsync(service, cancellationToken);
        return service;
    }
}