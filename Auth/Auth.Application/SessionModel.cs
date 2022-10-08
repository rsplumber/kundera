namespace Auth.Application;

public sealed record SessionModel
{
    public string Scope { get; init; } = string.Empty;

    public Guid UserId { get; init; }

    public DateTime ExpireDateUtc { get; init; }

    public DateTime LastUsageDateUtc { get; init; }

    public string LastIpAddress { get; init; } = string.Empty;
}