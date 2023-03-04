using System.Text.Json;
using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Sessions;
using Core.Domains.Roles;
using Core.Domains.Services;
using Core.Domains.Users;
using Microsoft.Extensions.Caching.Distributed;

namespace Data.Auth;

internal sealed class CachedAuthorizeDataProvider : IAuthorizeDataProvider
{
    private readonly IDistributedCache _cache;
    private readonly AuthorizeDataProvider _authorizeDataProvider;

    public CachedAuthorizeDataProvider(IDistributedCache cache, AuthorizeDataProvider authorizeDataProvider)
    {
        _cache = cache;
        _authorizeDataProvider = authorizeDataProvider;
    }

    public async Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        var cachedData = await _cache.GetAsync(sessionToken, cancellationToken);
        if (cachedData is not null)
        {
            return JsonSerializer.Deserialize<Session>(cachedData);
        }


        var session = await _authorizeDataProvider.CurrentSessionAsync(sessionToken, cancellationToken);
        await _cache.SetAsync(sessionToken, JsonSerializer.SerializeToUtf8Bytes(session), token: cancellationToken);
        return session;
    }

    public async Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        var cachedData = await DistributedCacheExtensions.GetStringAsync(_cache, user.Id.ToString(), cancellationToken);
        if (cachedData is not null)
        {
            return JsonSerializer.Deserialize<List<Role>>(cachedData);
        }

        var roles = await _authorizeDataProvider.UserRolesAsync(user, cancellationToken);
        await DistributedCacheExtensions.SetStringAsync(_cache, user.Id.ToString(), JsonSerializer.Serialize(roles), token: cancellationToken);
        return roles;
    }

    public async Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        var cachedData = await DistributedCacheExtensions.GetStringAsync(_cache, serviceSecret, cancellationToken);
        if (cachedData is not null)
        {
            return JsonSerializer.Deserialize<Service>(cachedData);
        }

        var service = await _authorizeDataProvider.RequestedServiceAsync(serviceSecret, cancellationToken);
        await DistributedCacheExtensions.SetStringAsync(_cache, serviceSecret, JsonSerializer.Serialize(service), token: cancellationToken);
        return service;
    }
}