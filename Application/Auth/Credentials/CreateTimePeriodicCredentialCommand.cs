﻿using Core.Domains.Auth.Credentials;
using Core.Domains.Users.Types;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record CreateTimePeriodicCredentialCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; }

    public bool? SingleSession { get; set; }
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
            UserId.From(command.UserId),
            command.ExpireInMinutes,
            command.SingleSession
        );

        return Unit.Value;
    }
}