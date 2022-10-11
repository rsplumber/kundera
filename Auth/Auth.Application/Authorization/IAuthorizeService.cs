using System.Net;
using Auth.Domain.Sessions;

namespace Auth.Application.Authorization;

public interface IAuthorizeService
{
    ValueTask AuthorizeAsync(Token token,
        string action,
        string scope,
        string? service,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}