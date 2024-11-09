using Core.Auth.Authorizations;
using Core.Auth.Credentials;
using FastEndpoints;
using FluentValidation;

namespace Application.Auth.Authentications.RefreshToken;

file sealed class Endpoint : Endpoint<Request, Certificate>
{
    private readonly IAuthenticateHandler _authenticateHandler;

    public Endpoint(IAuthenticateHandler authenticateHandler)
    {
        _authenticateHandler = authenticateHandler;
    }

    public override void Configure()
    {
        Post("authenticate/refresh");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var certificate = await _authenticateHandler.RefreshAsync(
            Certificate.From(req.Token, req.RefreshToken),
            new RequestInfo
            {
                UserAgent = HttpContext.Request.UserAgent(),
                IpAddress = HttpContext.Request.IpAddress()
            },
            ct);
        await SendOkAsync(certificate, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Refresh token";
        Description = "Refresh expired token";
        Response<Certificate>(200, "Expired token refreshed");
    }
}

file sealed record Request
{
    [FromHeader("Authorization")] public string Token { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}

file sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Enter valid Token")
            .NotNull().WithMessage("Enter valid Token");

        RuleFor(request => request.RefreshToken)
            .NotEmpty().WithMessage("Enter valid RefreshToken")
            .NotNull().WithMessage("Enter valid RefreshToken");
    }
}