using Core.Auth.Credentials;
using FastEndpoints;
using FluentValidation;

namespace Application.Auth.Logout;

file sealed class Endpoint : Endpoint<Request>
{
    private readonly IAuthenticateHandler _authenticateHandler;

    public Endpoint(IAuthenticateHandler authenticateHandler)
    {
        _authenticateHandler = authenticateHandler;
    }

    public override void Configure()
    {
        Post("logout");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _authenticateHandler.LogoutAsync(req.Token, req.RefreshToken, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Logout";
        Description = "Logout";
        Response(200, "Logout successfully");
    }
}

file sealed record Request
{
    public string Token { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}

file sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Enter valid Token")
            .NotNull().WithMessage("Enter Token");

        RuleFor(request => request.RefreshToken)
            .NotEmpty().WithMessage("Enter valid RefreshToken")
            .NotNull().WithMessage("Enter RefreshToken");
    }
}