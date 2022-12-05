using Application.Auth.Authentications;
using Core.Services;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Authentications;

internal sealed class AuthenticateEndpoint : Endpoint<AuthenticateRequest, Certificate>
{
    private readonly IMediator _mediator;

    public AuthenticateEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("authenticate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(AuthenticateRequest req, CancellationToken ct)
    {
        var command = new AuthenticateCommand
        {
            Username = req.Username,
            Password = req.Password,
            Type = req.Type,
            IpAddress = HttpContext.Connection.RemoteIpAddress,
            ScopeSecret = req.ScopeSecret
        };
        var response = await _mediator.Send(command, ct);

        await SendOkAsync(response, ct);
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
}