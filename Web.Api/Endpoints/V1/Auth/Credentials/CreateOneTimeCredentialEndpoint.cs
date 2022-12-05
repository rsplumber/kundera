using Application.Auth.Credentials;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Credentials;

internal sealed class CreateOneTimeCredentialEndpoint : Endpoint<CreateOneTimeCredentialRequest>
{
    private readonly IMediator _mediator;

    public CreateOneTimeCredentialEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{id:guid}/credentials/one-time");
        Permissions("credentials_create_onetime");
        Version(1);
    }

    public override async Task HandleAsync(CreateOneTimeCredentialRequest req, CancellationToken ct)
    {
        var command = new CreateOneTimeCredentialCommand
        {
            Username = req.Username,
            Password = req.Password,
            Type = req.Type,
            ExpireInMinutes = req.ExpireInMinutes,
            IpAddress = HttpContext.Connection.RemoteIpAddress,
            UserId = req.Id
        };

        await _mediator.Send(command, ct);
        await SendOkAsync(ct);
    }
}

internal sealed class CreateOneTimeCredentialEndpointSummary : Summary<CreateOneTimeCredentialEndpoint>
{
    public CreateOneTimeCredentialEndpointSummary()
    {
        Summary = "Create OneTime Credential";
        Description = "Create a OneTime Credential that expires after one use";
        Response(200, "Credential Created successfully");
    }
}

public record CreateOneTimeCredentialRequest
{
    public Guid Id { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string? Type { get; init; }

    public int ExpireInMinutes { get; init; } = 0;
}