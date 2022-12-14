using Core.Domains.Auth.Sessions;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Endpoints.V1.Auth.Sessions.Terminate;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly ISessionManagement _sessionManagement;

    public Endpoint(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    public override void Configure()
    {
        Delete("sessions/terminate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _sessionManagement.DeleteAsync(Token.From(req.Token), ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(204, "Session terminated successfully");
    }
}

internal sealed record Request
{
    public string Token { get; init; } = default!;
}

internal sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Enter valid Token")
            .NotNull().WithMessage("Enter valid Token");
    }
}