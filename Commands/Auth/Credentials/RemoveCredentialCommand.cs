using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Credentials.Exceptions;
using Mediator;

namespace Commands.Auth.Credentials;

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
        var credential = await _credentialRepository.FindAsync(command.Id, cancellationToken);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        await _credentialRepository.DeleteAsync(credential.Id, cancellationToken);

        return Unit.Value;
    }
}