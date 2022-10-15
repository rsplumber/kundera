using System.Net;
using Auth.Domain.Sessions;

namespace Auth.Application.Authorization;

public interface IAuthorizeService
{
    ValueTask<Guid> AuthorizeAsync(Token token,
        string action,
        string? scope,
        string? service,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}