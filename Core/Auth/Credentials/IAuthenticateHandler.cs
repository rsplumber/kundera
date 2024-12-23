using System.Net;
using Core.Auth.Authorizations;

namespace Core.Auth.Credentials;

public interface IAuthenticateHandler
{
    Task<Certificate> AuthenticateAsync(string username, string password, string scopeSecret, RequestInfo? requestInfo = null, CancellationToken cancellationToken = default);

    Task<Certificate> RefreshAsync(Certificate certificate, RequestInfo? requestInfo = null, CancellationToken cancellationToken = default);

    Task LogoutAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
}

public sealed record RequestInfo
{
    public string? UserAgent { get; init; }

    public IPAddress? IpAddress { get; init; }
}