﻿using Core.Auth.Credentials;
using Core.Auth.Credentials.Exceptions;
using Core.Auth.Sessions;
using Mediator;

namespace Application.Auth.Credentials.Password.Change;

public sealed record CredentialChangePasswordCommand : ICommand
{
    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string NewPassword { get; init; } = default!;
}

internal sealed class CredentialChangePasswordCommandHandler : ICommandHandler<CredentialChangePasswordCommand>
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly ISessionRepository _sessionRepository;

    public CredentialChangePasswordCommandHandler(ICredentialRepository credentialRepository, ISessionRepository sessionRepository)
    {
        _credentialRepository = credentialRepository;
        _sessionRepository = sessionRepository;
    }

    public async ValueTask<Unit> Handle(CredentialChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var credentials = await _credentialRepository.FindByUsernameAsync(command.Username, cancellationToken);
        var credential = credentials.FirstOrDefault(credential => credential.Password.Check(command.Password));
        if (credential is null) throw new CredentialNotFoundException();
        var sessions = await _sessionRepository.FindByCredentialIdAsync(credential.Id, cancellationToken);
        foreach (var session in sessions)
        {
            await _sessionRepository.DeleteAsync(session.Id, cancellationToken);
        }

        credential.ChangePassword(command.Password, command.NewPassword);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);
        return Unit.Value;
    }
}