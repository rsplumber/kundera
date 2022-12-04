using Core.Domains.Auth.Credentials;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record ValidateCredentialCommand : ICommand<Credential?>
{
    public string Username { get; set; } = default!;

    public string? Type { get; set; }
}

internal sealed class ValidateCredentialCommandHandler : ICommandHandler<ValidateCredentialCommand, Credential?>
{
    private readonly ICredentialRepository _credentialRepository;

    public ValidateCredentialCommandHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async ValueTask<Credential?> Handle(ValidateCredentialCommand command, CancellationToken cancellationToken)
    {
        var uniqueIdentifier = UniqueIdentifier.From(command.Username, command.Type);
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null) return null;

        if (Expired())
        {
            await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
            return null;
        }

        if (!credential.OneTime) return credential;
        await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
        return credential;

        bool Expired() => DateTime.UtcNow >= credential.ExpiresAt;
    }
}

public sealed class ValidateCredentialCommandValidator : AbstractValidator<ValidateCredentialCommand>
{
    public ValidateCredentialCommandValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");
    }
}