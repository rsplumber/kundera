using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials.OneTime;

file sealed class Endpoint : Endpoint<CreateOneTimeCredentialCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/credentials/one-time");
        Permissions("credentials_create_onetime");
        Version(1);
    }

    public override async Task HandleAsync(CreateOneTimeCredentialCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create OneTime Credential";
        Description = "Create a OneTime Credential that expires after one use";
        Response(200, "Credential Created successfully");
    }
}

file sealed class RequestValidator : Validator<CreateOneTimeCredentialCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");

        RuleFor(request => request.ExpireInMinutes)
            .NotEmpty().WithMessage("Enter valid ExpireInMinutes")
            .NotNull().WithMessage("Enter valid ExpireInMinutes")
            .GreaterThan(1).WithMessage("ExpireInMinutes must be greater than 1");
    }
}