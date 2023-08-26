using System.Net;

namespace Core.Auth.Authorizations;

public interface IPermissionAuthorizationHandler
{
    ValueTask<AuthorizeResponse> HandleAsync(
        string token,
        string serviceSecret,
        string[] actions,
        IPAddress ipAddress,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}