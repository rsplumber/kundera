using Core.Domains.Credentials;
using FluentValidation;
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

public sealed class RemoveCredentialCommandValidator : AbstractValidator<RemoveCredentialCommand>
{
    public RemoveCredentialCommandValidator()
    {
        RuleFor(request => request.UniqueIdentifier)
            .NotEmpty().WithMessage("Enter valid UniqueIdentifier")
            .NotNull().WithMessage("Enter valid UniqueIdentifier");
    }
}