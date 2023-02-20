﻿using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Credentials.Exceptions;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record RemoveCredentialCommand : ICommand
{
    public Guid Id { get; set; } = default!;
}

internal sealed class RemoveCredentialCommandHandler : ICommandHandler<RemoveCredentialCommand>
{
    private readonly ICredentialRepository _credentialRepository;

    public RemoveCredentialCommandHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async ValueTask<Unit> Handle(RemoveCredentialCommand command, CancellationToken cancellationToken)
    {
        var credentialId = CredentialId.From(command.Id);
        var credential = await _credentialRepository.FindAsync(credentialId, cancellationToken);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        await _credentialRepository.DeleteAsync(credentialId, cancellationToken);

        return Unit.Value;
    }
}