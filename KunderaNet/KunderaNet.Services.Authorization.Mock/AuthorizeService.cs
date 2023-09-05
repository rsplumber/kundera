using KunderaNet.Services.Authorization.Abstractions;
using Microsoft.Extensions.Configuration;

namespace KunderaNet.Services.Authorization.Mock;

internal sealed class AuthorizeService : IAuthorizeService
{
    private readonly string _userId;
    private readonly string _scopeId;
    private readonly string _serviceId;

    public AuthorizeService(IConfiguration configuration)
    {
        _userId = configuration.GetSection("Kundera:Fake:UserId").Value ?? Guid.Empty.ToString();
        _scopeId = configuration.GetSection("Kundera:Fake:Scope").Value ?? Guid.Empty.ToString();
        _serviceId = configuration.GetSection("Kundera:Fake:Service").Value ?? Guid.Empty.ToString();
    }


    public Task<(int, AuthorizedResponse?)> AuthorizePermissionAsync(string token,
        IEnumerable<string> permissions,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult((200, new AuthorizedResponse
        {
            UserId = _userId,
            ScopeId = _scopeId,
            ServiceId = _serviceId
        }))!;
    }

    public Task<(int, AuthorizedResponse?)> AuthorizeRoleAsync(string token,
        IEnumerable<string> roles,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult((200, new AuthorizedResponse
        {
            UserId = _userId,
            ScopeId = _scopeId,
            ServiceId = _serviceId
        }))!;
    }
}