using DotNetCore.CAP;

namespace Core.Auth.Credentials;

public record AuthenticatedEvent
{
    public const string EventName = "kundera.auth.authenticated";

    public Guid CredentialId { get; init; }

    public Guid UserId { get; init; }

    public Guid ScopeId { get; init; }

    public string Username { get; init; } = default!;

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }
    
    public string? Platform { get; init; }
}

public sealed class AuthenticatedEventHandler : ICapSubscribe
{
    private readonly IAuthenticationActivityRepository _authenticationActivityRepository;

    public AuthenticatedEventHandler(IAuthenticationActivityRepository authenticationActivityRepository)
    {
        _authenticationActivityRepository = authenticationActivityRepository;
    }

    [CapSubscribe(AuthenticatedEvent.EventName, Group = "kundera.authentication_activities.queue")]
    public async Task HandleAsync(AuthenticatedEvent message)
    {
        await _authenticationActivityRepository.AddAsync(new AuthenticationActivity(
            message.CredentialId,
            message.UserId,
            message.ScopeId,
            message.Username,
            message.IpAddress,
            message.Agent,
            message.Platform));
    }
}