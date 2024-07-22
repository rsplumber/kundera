using Core.Auth.Credentials;
using Mediator;

namespace Application.Auth.Credentials.Basic;

public sealed record CreateBasicCredentialCommand : ICommand
{
    public Guid UserId { get; init; } = default!;
    
    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public bool? SingleSession { get; init; }

    public int SessionTokenExpireTimeInMinutes { get; init; }

    public int SessionRefreshTokenExpireTimeInMinutes { get; init; }
}

internal sealed class CreateBasicCredentialCommandHandler : ICommandHandler<CreateBasicCredentialCommand>
{
    private readonly ICredentialFactory _credentialFactory;

    public CreateBasicCredentialCommandHandler(ICredentialFactory credentialFactory)
    {
        _credentialFactory = credentialFactory;
    }

    public async ValueTask<Unit> Handle(CreateBasicCredentialCommand command, CancellationToken cancellationToken)
    {
        await _credentialFactory.CreateAsync(
            command.UserId,
            command.Username,
            command.Password,
            command.SessionTokenExpireTimeInMinutes,
            command.SessionRefreshTokenExpireTimeInMinutes,
            command.SingleSession);

        return Unit.Value;
    }
}