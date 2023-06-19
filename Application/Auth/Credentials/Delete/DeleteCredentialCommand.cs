using Core.Auth.Credentials;
using Core.Auth.Credentials.Exceptions;
using Mediator;

namespace Application.Auth.Credentials.Delete;

public sealed record DeleteCredentialCommand : ICommand
{
    public Guid Id { get; set; } = default!;
}

internal sealed class DeleteCredentialCommandHandler : ICommandHandler<DeleteCredentialCommand>
{
    private readonly ICredentialRepository _credentialRepository;

    public DeleteCredentialCommandHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async ValueTask<Unit> Handle(DeleteCredentialCommand command, CancellationToken cancellationToken)
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