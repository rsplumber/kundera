using Core.Domains.Sessions;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Endpoints.V1.Auth.Sessions;

internal sealed class TerminateSessionEndpoint : Endpoint<TerminateSessionRequest>
{
    private readonly ISessionManagement _sessionManagement;

    public TerminateSessionEndpoint(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    public override void Configure()
    {
        Delete("sessions/terminate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(TerminateSessionRequest req, CancellationToken ct)
    {
        await _sessionManagement.DeleteAsync(Token.From(req.Token), ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class TerminateSessionEndpointSummary : Summary<TerminateSessionEndpoint>
{
    public TerminateSessionEndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(204, "Session terminated successfully");
    }
}

public sealed record TerminateSessionRequest
{
    public string Token { get; init; } = default!;
}

public class TerminateSessionRequestValidator : Validator<TerminateSessionRequest>
{
    public TerminateSessionRequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Enter valid Token")
            .NotNull().WithMessage("Enter valid Token");
    }
}