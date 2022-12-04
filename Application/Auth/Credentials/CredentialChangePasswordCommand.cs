using System.Net;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Credentials.Exceptions;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record CredentialChangePasswordCommand : ICommand
{
    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string NewPassword { get; init; } = default!;

    public string? Type { get; init; }

    public IPAddress? IpAddress { get; init; }
}

internal sealed class CredentialChangePasswordCommandHandler : ICommandHandler<CredentialChangePasswordCommand>
{
    private readonly ICredentialRepository _credentialRepository;


    public CredentialChangePasswordCommandHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async ValueTask<Unit> Handle(CredentialChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var credential = await _credentialRepository.FindAsync(UniqueIdentifier.From(command.Username, command.Type), cancellationToken);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        credential.ChangePassword(command.Password, command.NewPassword);
        credential.UpdateActivityInfo(command.IpAddress ?? IPAddress.None);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);

        return Unit.Value;
    }
}

public sealed class CredentialChangePasswordCommandValidator : AbstractValidator<CredentialChangePasswordCommand>
{
    public CredentialChangePasswordCommandValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");

        RuleFor(request => request.NewPassword)
            .NotEmpty().WithMessage("Enter valid NewPassword")
            .NotNull().WithMessage("Enter valid NewPassword");
    }
}