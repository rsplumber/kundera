using Application.Auth.Authentications;
using Core.Services;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Authentications;

internal sealed class RefreshTokenEndpoint : Endpoint<RefreshTokenRequest, Certificate>
{
    private readonly IMediator _mediator;

    public RefreshTokenEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("authenticate/refresh");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
    {
        var command = new RefreshCertificateCommand
        {
            Token = req.Authorization,
            RefreshToken = req.RefreshToken,
            IpAddress = HttpContext.Connection.RemoteIpAddress
        };
        var response = await _mediator.Send(command, ct);

        await SendOkAsync(response, ct);
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