using Mediator;

namespace Data.Abstractions.Users;

public sealed record UserAuthenticateActivitiesQuery : PageableQuery, IQuery<PageableResponse<UserAuthenticateActivitiesResponse>>
{
    public Guid UserId { get; set; }
}

public sealed record UserAuthenticateActivitiesResponse
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string? Username { get; init; }

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }
    
    public DateTime CreatedDateUtc { get; init; }
};