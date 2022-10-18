using System.Net;

namespace Auth.Core.Services;

public interface IAuthorizeService
{
    Task<Guid> AuthorizeAsync(Token token,
        string action,
        string? scope,
        string? service,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}