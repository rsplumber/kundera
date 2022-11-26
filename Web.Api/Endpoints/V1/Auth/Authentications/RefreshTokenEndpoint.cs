using Core.Domains.Sessions;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Endpoints.V1.Auth.Authentications;

internal sealed class RefreshTokenEndpoint : Endpoint<RefreshTokenRequest, Certificate>
{
    private readonly IAuthenticateService _authenticateService;

    public RefreshTokenEndpoint(IAuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    public override void Configure()
    {
        Post("authenticate/refresh");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
    {
        var certificate = await _authenticateService.RefreshCertificateAsync(
            Token.From(req.Authorization),
            Token.From(req.RefreshToken),
            HttpContext.Connection.LocalIpAddress,
            ct);

        await SendOkAsync(certificate, ct);
    }
}

internal sealed class RefreshTokenEndpointSummary : Summary<RefreshTokenEndpoint>
{
    public RefreshTokenEndpointSummary()
    {
        Summary = "Refresh token";
        Description = "Refresh expired token";
        Response<Certificate>(200, "Expired token refreshed");
    }
}

public sealed record RefreshTokenRequest
{
    [FromHeader] public string Authorization { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}

internal sealed class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty().WithMessage("Enter valid RefreshToken")
            .NotNull().WithMessage("Enter valid RefreshToken");
    }
}