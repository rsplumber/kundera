using Core.Auth.Credentials;
using Mediator;

namespace Application.Auth.Credentials.OneTime;

public sealed record CreateOneTimeCredentialCommand : ICommand
{
    public Guid UserId { get; init; } = default!;
    
    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; }

    public int SessionTokenExpireTimeInMinutes { get; init; }

    public int SessionRefreshTokenExpireTimeInMinutes { get; init; }
}

internal sealed class CreateOneTimeCredentialCommandHandler : ICommandHandler<CreateOneTimeCredentialCommand>
{
    private readonly ICredentialFactory _credentialFactory;

    public CreateOneTimeCredentialCommandHandler(ICredentialFactory credentialFactory)
    {
        _credentialFactory = credentialFactory;
    }

    public async ValueTask<Unit> Handle(CreateOneTimeCredentialCommand command, CancellationToken cancellationToken)
    {
        await _credentialFactory.CreateOneTimeAsync(
            command.UserId,
            command.Username,
            command.Password,
            command.SessionTokenExpireTimeInMinutes,
            command.SessionRefreshTokenExpireTimeInMinutes,
            command.ExpireInMinutes);

        return Unit.Value;
    }
}