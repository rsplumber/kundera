namespace Core.Auth.Authorizations;

public interface IAuthorizeService
{
    Task<(Guid?, UnAuthorizeResponse?)> AuthorizePermissionAsync(string token,
        IEnumerable<string> actions,
        string serviceSecret,
        string? userAgent,
        string ipAddress,
        CancellationToken cancellationToken = default);

    Task<(Guid?, UnAuthorizeResponse?)> AuthorizeRoleAsync(string token,
        IEnumerable<string> roles,
        string serviceSecret,
        string? userAgent,
        string ipAddress,
        CancellationToken cancellationToken = default);
}

public record UnAuthorizeResponse(int Code)
{
    public static readonly UnAuthorizeResponse UnAuthorized = new(401);
    public static readonly UnAuthorizeResponse Forbidden = new(403);
    public static readonly UnAuthorizeResponse SessionExpired = new(440);
}