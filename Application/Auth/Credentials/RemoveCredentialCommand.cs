using Core.Domains.Auth.Credentials;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record RemoveCredentialCommand : ICommand
{
    public string UniqueIdentifier { get; set; } = default!;
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
        await _credentialRepository.DeleteAsync(UniqueIdentifier.Parse(command.UniqueIdentifier), cancellationToken);

        return Unit.Value;
    }
}