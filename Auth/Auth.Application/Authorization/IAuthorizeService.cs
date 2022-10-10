using System.Net;
using Auth.Domain.Sessions;

namespace Auth.Application.Authorization;

public interface IAuthorizeService
{
    ValueTask AuthorizeAsync(Token token,
        string action,
        string scope,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}