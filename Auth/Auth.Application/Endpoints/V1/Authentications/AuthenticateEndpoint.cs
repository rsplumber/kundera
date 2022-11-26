using Core.Domains.Credentials;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Auth.Application.Endpoints.V1.Authentications;

internal sealed class AuthenticateEndpoint : Endpoint<AuthenticateRequest, Certificate>
{
    private readonly IAuthenticateService _authenticateService;

    public AuthenticateEndpoint(IAuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    public override void Configure()
    {
        Post("authenticate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(AuthenticateRequest req, CancellationToken ct)
    {
        var uniqueIdentifier = UniqueIdentifier.From(req.Username, req.Type);
        var certificate = await _authenticateService.AuthenticateAsync(uniqueIdentifier,
            req.Password,
            req.ScopeSecret,
            HttpContext.Connection.LocalIpAddress,
            ct);

        await SendOkAsync(certificate, ct);
    }
}

internal sealed class AuthenticateEndpointSummary : Summary<AuthenticateEndpoint>
{
    public AuthenticateEndpointSummary()
    {
        Summary = "Authenticate";
        Description = "Authenticate by username and password";
        Response<Certificate>(200, "Client authenticated successfully");
    }
}

public sealed record AuthenticateRequest
{
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? Type { get; set; }

    [FromHeader] public string ScopeSecret { get; set; } = default!;
};

internal sealed class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequest>
{
    public AuthenticateRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty()
            .WithMessage("Enter valid Username")
            .NotNull()
            .WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Enter valid Password")
            .NotNull()
            .WithMessage("Enter valid Password");

        RuleFor(request => request.ScopeSecret)
            .NotEmpty()
            .WithMessage("Enter valid ScopeSecret")
            .NotNull()
            .WithMessage("Enter valid ScopeSecret");
    }
}