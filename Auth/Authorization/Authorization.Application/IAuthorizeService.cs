using System.Net;
using Authorization.Domain.Types;

namespace Authorization.Application;

public interface IAuthorizeService
{
    ValueTask<bool> AuthorizeAsync(Token token, IPAddress? ipAddress, CancellationToken cancellationToken = default);
}