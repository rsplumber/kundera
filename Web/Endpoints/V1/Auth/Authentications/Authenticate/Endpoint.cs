using Commands.Auth.Authentications;
using Core.Domains.Auth.Authorizations;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Auth.Authentications.Authenticate;

internal sealed class Endpoint : Endpoint<Request, Certificate>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("authenticate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = new AuthenticateCommand
        {
            Username = req.Username,
            Password = req.Password,
            ScopeSecret = req.ScopeSecret,
            UserAgent = HttpContext.Request.UserAgent()
        };
        var response = await _mediator.Send(command, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Authenticate";
        Description = "Authenticate by username and password";
        Response<Certificate>(200, "Client authenticated successfully");
    }
}

internal sealed record Request
{
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    [FromHeader("scope_secret")] public string ScopeSecret { get; set; } = default!;
}

internal sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");

        RuleFor(request => request.ScopeSecret)
            .NotEmpty().WithMessage("Enter valid ScopeSecret")
            .NotNull().WithMessage("Enter valid ScopeSecret");
    }
}