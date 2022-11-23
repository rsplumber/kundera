using System.Net;
using Auth.Core.Entities;

namespace Auth.Core.Services;

public interface IAuthorizeService
{
    Task<Guid> AuthorizeAsync(Token token,
        string action,
        string serviceSecret,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}