using System.Net;

namespace Core.Services;

public interface IAuthorizeService
{
    Task<Guid> AuthorizeAsync(string token,
        string action,
        string serviceSecret,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}