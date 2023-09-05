namespace KunderaNet.Services.Authorization.Abstractions;

public record AuthorizedResponse
{
    public string UserId { get; init; } = default!;

    public string? ScopeId { get; init; }

    public string? ServiceId { get; init; }
}