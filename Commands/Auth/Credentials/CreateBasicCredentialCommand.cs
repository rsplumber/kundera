using Core.Domains.Auth.Credentials;
using Mediator;

namespace Commands.Auth.Credentials;

public sealed record CreateBasicCredentialCommand : ICommand
{
    public Guid UserId { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public bool? SingleSession { get; set; }
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
        await _credentialFactory.CreateAsync(command.Username,
            command.Password,
            command.UserId,
            command.SingleSession);

        return Unit.Value;
    }
}