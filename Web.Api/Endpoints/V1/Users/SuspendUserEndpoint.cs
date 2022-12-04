using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class SuspendUserEndpoint : Endpoint<ActiveUserCommand>
{
    private readonly IMediator _mediator;

    public SuspendUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{id:guid}/suspend");
        Permissions("user_suspend");
        Version(1);
    }

    public override async Task HandleAsync(ActiveUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class SuspendUserEndpointSummary : Summary<SuspendUserEndpoint>
{
    public SuspendUserEndpointSummary()
    {
        Summary = "Suspend a user in the system";
        Description = "Suspend a user in the system";
        Response(200, "Users was successfully Suspended");
    }
}