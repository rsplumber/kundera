using Core.Auth.Credentials;
using Mediator;

namespace Application.Auth.Credentials.TimePeriodic;

public sealed record CreateTimePeriodicCredentialCommand : ICommand
{
    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; }

    public bool? SingleSession { get; set; }

    public int SessionTokenExpireTimeInMinutes { get; init; }

    public int SessionRefreshTokenExpireTimeInMinutes { get; init; }
}

internal sealed class CreateTimePeriodicCredentialCommandHandler : ICommandHandler<CreateTimePeriodicCredentialCommand>
{
    private readonly ICredentialFactory _credentialFactory;

    public CreateTimePeriodicCredentialCommandHandler(ICredentialFactory credentialFactory)
    {
        _credentialFactory = credentialFactory;
    }

    public async ValueTask<Unit> Handle(CreateTimePeriodicCredentialCommand command, CancellationToken cancellationToken)
    {
        await _credentialFactory.CreateTimePeriodicAsync(command.Username,
            command.Password,
            command.SessionTokenExpireTimeInMinutes,
            command.SessionRefreshTokenExpireTimeInMinutes,
            command.ExpireInMinutes,
            command.SingleSession
        );

        return Unit.Value;
    }
}