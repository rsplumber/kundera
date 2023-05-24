using Core.Domains.Auth.Sessions;
using DotNetCore.CAP;

namespace Core.Domains.Auth.Authorizations;

public record AuthorizedEvent
{
    public const string EventName = "kundera.auth.authorized";

    public string SessionId { get; init; } = default!;

    public Guid UserId { get; init; }

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }
}

public sealed class AuthorizedEventHandler : ICapSubscribe
{
    private readonly IAuthorizationActivityRepository _authorizationActivityRepository;

    public AuthorizedEventHandler(IAuthorizationActivityRepository authorizationActivityRepository)
    {
        _authorizationActivityRepository = authorizationActivityRepository;
    }


    [CapSubscribe(AuthorizedEvent.EventName, Group = "kundera.core.queue")]
    public async Task HandleAsync(AuthorizedEvent message)
    {
        await _authorizationActivityRepository.AddAsync(new AuthorizationActivity(message.SessionId, message.UserId, message.IpAddress, message.Agent));
    }
}