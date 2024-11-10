using Core.Auth.Credentials;
using Core.Auth.Credentials.Exceptions;
using Mediator;

namespace Application.Auth.Credentials.Username.Change;

public sealed record CredentialChangeUsernameCommand : ICommand
{
    public string OldUsername { get; init; } = default!;

    public string Username { get; init; } = default!;
}

internal sealed class CredentialChangeUsernameCommandHandler : ICommandHandler<CredentialChangeUsernameCommand>
{
    private readonly ICredentialRepository _credentialRepository;

    public CredentialChangeUsernameCommandHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async ValueTask<Unit> Handle(CredentialChangeUsernameCommand command, CancellationToken cancellationToken)
    {
        var credentials = await _credentialRepository.FindByUsernameAsync(command.OldUsername, cancellationToken);
        var credential = credentials.FirstOrDefault();
        if (credential is null) throw new CredentialNotFoundException();
        credential.ChangeUsername(command.Username);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);
        return Unit.Value;
    }
}