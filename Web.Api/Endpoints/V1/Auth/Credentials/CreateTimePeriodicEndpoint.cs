using Application.Auth.Credentials;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Credentials;

internal sealed class CreateTimePeriodicEndpoint : Endpoint<CreateTimePeriodicCredentialRequest>
{
    private readonly IMediator _mediator;

    public CreateTimePeriodicEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{id:guid}/credentials/time-periodic");
        Permissions("credentials_create_time-periodic");
        Version(1);
    }

    public override async Task HandleAsync(CreateTimePeriodicCredentialRequest req, CancellationToken ct)
    {
        var command = new CreateTimePeriodicCredentialCommand
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

internal sealed class CreateTimePeriodicEndpointSummary : Summary<CreateTimePeriodicEndpoint>
{
    public CreateTimePeriodicEndpointSummary()
    {
        Summary = "Create TimePeriodic Credential";
        Description = "Create a TimePeriodic Credential that expires after period of time";
        Response(200, "Credential Created successfully");
    }
}

public record CreateTimePeriodicCredentialRequest
{
    public Guid Id { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; } = default!;

    public string? Type { get; init; } = default;
}