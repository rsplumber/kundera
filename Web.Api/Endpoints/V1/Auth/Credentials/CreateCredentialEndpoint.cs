using Application.Auth.Credentials;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Credentials;

internal sealed class CreateCredentialEndpoint : Endpoint<CreateCredentialRequest>
{
    private readonly IMediator _mediator;

    public CreateCredentialEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{id:guid}/credentials");
        Permissions("credentials_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateCredentialRequest req, CancellationToken ct)
    {
        var command = new CreateBasicCredentialCommand
        {
            Username = req.Username,
            Password = req.Password,
            Type = req.Type,
            IpAddress = HttpContext.Connection.RemoteIpAddress,
            UserId = req.Id
        };

        await _mediator.Send(command, ct);
        await SendOkAsync(ct);
    }
}

internal sealed class CreateCredentialEndpointSummary : Summary<CreateCredentialEndpoint>
{
    public CreateCredentialEndpointSummary()
    {
        Summary = "Create Credential";
        Description = "Create a default Credential";
        Response(200, "Credential Created successfully");
    }
}

public sealed record CreateCredentialRequest
{
    public Guid Id { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? Type { get; set; }
}