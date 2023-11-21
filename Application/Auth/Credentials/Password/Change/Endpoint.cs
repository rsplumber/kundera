using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials.Password.Change;

file sealed class Endpoint : Endpoint<CredentialChangePasswordCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/credentials/password/change");
        Permissions("credentials_password_change");
        Version(1);
    }

    public override async Task HandleAsync(CredentialChangePasswordCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Change credential password";
        Description = "Change credential password";
        Response(200, "Successful");
    }
}

file sealed class RequestValidator : Validator<CredentialChangePasswordCommand>
{
    public RequestValidator()
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