using System.Net;
using Auth.Application;
using Auth.Domain.Sessions;

namespace Authorization.Infrastructure;

internal sealed class AuthorizeService : IAuthorizeService
{
    private readonly ISessionManagement _sessionManagement;

    public AuthorizeService(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    public async ValueTask<bool> AuthorizeAsync(Token token, IPAddress? ipAddress, CancellationToken cancellationToken = default)
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

        return true;

        bool TokenExpired() => session.ExpireDateUtc >= DateTime.UtcNow;
    }
}