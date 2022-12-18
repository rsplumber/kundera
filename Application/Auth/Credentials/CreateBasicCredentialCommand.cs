using System.Net;
using Core.Domains.Auth.Credentials;
using Core.Domains.Users.Types;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record CreateBasicCredentialCommand : ICommand
{
    public Guid UserId { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? Type { get; set; }

    public IPAddress? IpAddress { get; set; }
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
        await _credentialFactory.CreateAsync(UniqueIdentifier.From(command.Username, command.Type),
            command.Password,
            UserId.From(command.UserId),
            command.IpAddress);

        return Unit.Value;
    }
}