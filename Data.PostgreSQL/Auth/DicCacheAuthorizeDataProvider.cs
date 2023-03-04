using System.Collections.Concurrent;
using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Sessions;
using Core.Domains.Roles;
using Core.Domains.Services;
using Core.Domains.Users;

namespace Data.Auth;

internal sealed class DicCacheAuthorizeDataProvider : IAuthorizeDataProvider
{
    private static readonly ConcurrentDictionary<string, Session?> Sessions = new();
    private static readonly ConcurrentDictionary<string, List<Role>?> Roles = new();
    private static readonly ConcurrentDictionary<string, Service?> Services = new();

    private readonly AuthorizeDataProvider _authorizeDataProvider;

    public DicCacheAuthorizeDataProvider(AuthorizeDataProvider authorizeDataProvider)
    {
        _authorizeDataProvider = authorizeDataProvider;
    }

    public async Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        Sessions.TryGetValue(sessionToken, out var cachedData);
        if (cachedData is not null)
        {
            return cachedData;
        }


        var session = await _authorizeDataProvider.CurrentSessionAsync(sessionToken, cancellationToken);
        Sessions.TryAdd(sessionToken, session);
        return session;
    }

    public async Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        Roles.TryGetValue(user.Id.ToString(), out var cachedData);
        if (cachedData is not null)
        {
            return cachedData;
        }

        var roles = await _authorizeDataProvider.UserRolesAsync(user, cancellationToken);
        Roles.TryAdd(user.Id.ToString(), roles);
        return roles;
    }

    public async Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        Services.TryGetValue(serviceSecret, out var cachedData);
        if (cachedData is not null)
        {
            return cachedData;
        }

        var service = await _authorizeDataProvider.RequestedServiceAsync(serviceSecret, cancellationToken);
        Services.TryAdd(serviceSecret, service);
        return service;
    }
}