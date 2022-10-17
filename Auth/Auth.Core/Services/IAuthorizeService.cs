using System.Net;
using Auth.Core.Domains;

namespace Auth.Core.Services;

public interface IAuthorizeService
{
    ValueTask<Guid> AuthorizeAsync(Token token,
        string action,
        string? scope,
        string? service,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}