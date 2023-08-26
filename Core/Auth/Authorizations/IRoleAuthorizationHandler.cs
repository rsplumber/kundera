using System.Net;

namespace Core.Auth.Authorizations;

public interface IRoleAuthorizationHandler
{
    ValueTask<AuthorizeResponse> HandleAsync(
        string token,
        string serviceSecret,
        string[] roles,
        IPAddress ipAddress,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}