using Core.Auth.Authorizations;
using Core.Auth.Credentials;
using FastEndpoints;
using FluentValidation;

namespace Application.Auth.Authentications.Authenticate;

file sealed class Endpoint : Endpoint<Request, Certificate>
{
    private readonly IAuthenticateHandler _authenticateHandler;

    public Endpoint(IAuthenticateHandler authenticateHandler)
    {
        _authenticateHandler = authenticateHandler;
    }

    public override void Configure()
    {
        Post("authenticate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var certificate = await _authenticateHandler.AuthenticateAsync(req.Username,
            req.Password,
            req.ScopeSecret,
            new RequestInfo
            {
                UserAgent = HttpContext.Request.UserAgent(),
                IpAddress = HttpContext.Request.IpAddress(),
            },
            ct);
        await SendOkAsync(certificate, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Authenticate";
        Description = "Authenticate by username and password";
        Response<Certificate>(200, "Client authenticated successfully");
    }
}

file sealed record Request
{
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    [FromHeader("scope_secret")] public string ScopeSecret { get; set; } = default!;
}

file sealed class RequestValidator : Validator<Request>
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