using System.Net;

namespace Core.Services;

public interface IAuthorizeService
{
    Task<Guid> AuthorizePermissionAsync(string token,
        IEnumerable<string> actions,
        string serviceSecret,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);

    Task<Guid> AuthorizeRoleAsync(string token,
        IEnumerable<string> roles,
        string serviceSecret,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);
}