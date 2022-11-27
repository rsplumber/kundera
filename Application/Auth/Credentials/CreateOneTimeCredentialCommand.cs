using System.Net;
using Core.Domains.Credentials;
using Core.Domains.Users.Types;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record CreateOneTimeCredentialCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; } = 0;

    public string? Type { get; init; }

    public IPAddress? IpAddress { get; init; }
}

internal sealed class CreateOneTimeCredentialCommandHandler : ICommandHandler<CreateOneTimeCredentialCommand>
{
    private readonly ICredentialFactory _credentialFactory;

    public CreateOneTimeCredentialCommandHandler(ICredentialFactory credentialFactory)
    {
        _credentialFactory = credentialFactory;
    }

    public async ValueTask<Unit> Handle(CreateOneTimeCredentialCommand command, CancellationToken cancellationToken)
    {
        await _credentialFactory.CreateAsync(UniqueIdentifier.From(command.Username, command.Type),
            command.Password,
            UserId.From(command.UserId),
            command.IpAddress,
            command.ExpireInMinutes,
            true);

        return Unit.Value;
    }
}

public sealed class CreateOneTimeCredentialCommandValidator : AbstractValidator<CreateOneTimeCredentialCommand>
{
    public CreateOneTimeCredentialCommandValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter valid UserId")
            .NotNull().WithMessage("Enter valid UserId");

        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");
    }
}