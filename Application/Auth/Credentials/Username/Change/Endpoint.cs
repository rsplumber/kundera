using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials.Username.Change;

file sealed class Endpoint : Endpoint<CredentialChangeUsernameCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/credentials/username/change");
        Permissions("credentials_username_change");
        Version(1);
    }

    public override async Task HandleAsync(CredentialChangeUsernameCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Change credential username";
        Description = "Change credential username";
        Response(200, "Successful");
    }
}

file sealed class RequestValidator : Validator<CredentialChangeUsernameCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");

        RuleFor(request => request.OldUsername)
            .NotEmpty().WithMessage("Enter valid OldUsername")
            .NotNull().WithMessage("Enter valid OldUsername");
        
    }
}