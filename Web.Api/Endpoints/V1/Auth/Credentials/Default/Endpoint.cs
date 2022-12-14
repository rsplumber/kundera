using Application.Auth.Credentials;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Credentials.Default;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/credentials");
        Permissions("credentials_create");
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = new CreateBasicCredentialCommand
        {
            Username = req.Username,
            Password = req.Password,
            Type = req.Type,
            IpAddress = HttpContext.Connection.RemoteIpAddress,
            UserId = req.UserId
        };

        await _mediator.Send(command, ct);
        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create Credential";
        Description = "Create a default Credential";
        Response(200, "Credential Created successfully");
    }
}

internal sealed record Request
{
    public Guid UserId { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? Type { get; set; }
}