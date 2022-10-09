using System.Net;
using Auth.Domain.Sessions;

namespace Auth.Application.Authorization;

public interface IAuthorizeService
{
    ValueTask<bool> AuthorizeAsync(Token token,
        string action,
        string scope,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}