namespace KunderaNet.Services.Authorization.Abstractions;

public interface IAuthorizeService
{
    Task<(int, AuthorizedResponse?)> AuthorizePermissionAsync(string token,
        IEnumerable<string> permissions,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default);

    Task<(int, AuthorizedResponse?)> AuthorizeRoleAsync(string token,
        IEnumerable<string> roles,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default);
}