using Application.Auth.Credentials;
using FastEndpoints;
using FluentValidation;
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
        Post("users/{UserId:guid}/credentials/time-periodic");
        AllowAnonymous();
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
            IpAddress = HttpContext.Connection.LocalIpAddress,
            UserId = req.UserId
        };

        await _mediator.Send(command, ct);
        await SendOkAsync(ct);
    }
}

internal sealed class CreateTimePeriodicEndpointSummary : Summary<CreateTimePeriodicEndpoint>
{
    public CreateTimePeriodicEndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(200, "Session terminated successfully");
    }
}

public record CreateTimePeriodicCredentialRequest
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; } = default!;

    public string? Type { get; init; }
}
