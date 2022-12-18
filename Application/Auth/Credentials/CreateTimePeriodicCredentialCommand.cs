using System.Net;
using Core.Domains.Auth.Credentials;
using Core.Domains.Users.Types;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record CreateTimePeriodicCredentialCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; } = default!;

    public string? Type { get; init; }

    public IPAddress? IpAddress { get; init; }
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
        await _credentialFactory.CreateAsync(UniqueIdentifier.From(command.Username, command.Type),
            command.Password,
            UserId.From(command.UserId),
            command.IpAddress,
            command.ExpireInMinutes);

        return Unit.Value;
    }
}