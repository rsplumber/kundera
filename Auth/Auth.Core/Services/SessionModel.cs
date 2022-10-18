namespace Auth.Core.Services;

public sealed record SessionModel
{
    public string Scope { get; init; } = string.Empty;

    public Guid UserId { get; init; }

    public DateTime ExpiresAtUtc { get; init; }

    public DateTime LastUsageDateUtc { get; init; }

    public DateTime CreatedDateUtc { get; init; }

    public string LastIpAddress { get; init; } = string.Empty;
}