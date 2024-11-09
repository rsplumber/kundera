using System.Net;

namespace Core.Auth.Authorizations;

public interface IPermissionAuthorizationHandler
{
    ValueTask<AuthorizeResponse> HandleAsync(
        string token,
        string serviceSecret,
        IEnumerable<string> actions,
        IPAddress ipAddress,
        string? userAgent = null,
        string? platform = null,
        CancellationToken cancellationToken = default);
}