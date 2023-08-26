namespace Core.Auth.Authorizations;

public record AuthorizeResponse
{
    public static readonly AuthorizeResponse UnAuthorized = new(401);
    public static readonly AuthorizeResponse Forbidden = new(403);
    public static readonly AuthorizeResponse SessionExpired = new(440);

    private AuthorizeResponse(int code)
    {
        Code = code;
    }

    public static AuthorizeResponse Success(Guid userId, Guid scopeId, Guid serviceId) => new(200)
    {
        UserId = userId,
        ScopeId = scopeId,
        ServiceId = serviceId
    };

    public int Code { get; }

    public Guid? UserId { get; private set; }

    public Guid? ScopeId { get; private set; }

    public Guid? ServiceId { get; private set; }
}