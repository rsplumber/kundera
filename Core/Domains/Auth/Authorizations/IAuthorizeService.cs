namespace Core.Domains.Auth.Authorizations;

public interface IAuthorizeService
{
    Task<Guid> AuthorizePermissionAsync(string token,
        IEnumerable<string> actions,
        string serviceSecret,
        CancellationToken cancellationToken = default);

    Task<Guid> AuthorizeRoleAsync(string token,
        IEnumerable<string> roles,
        string serviceSecret,
        CancellationToken cancellationToken = default);
}