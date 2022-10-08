using System.Net;
using Auth.Domain.Sessions;

namespace Auth.Application;

public interface IAuthorizeService
{
    ValueTask<bool> AuthorizeAsync(Token token, IPAddress? ipAddress, CancellationToken cancellationToken = default);
}