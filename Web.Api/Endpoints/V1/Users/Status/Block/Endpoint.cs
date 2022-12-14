using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users.Status.Block;

internal sealed class Endpoint : Endpoint<ActiveUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/block");
        Permissions("user_block");
        Version(1);
    }

    public override async Task HandleAsync(ActiveUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Block a user in the system";
        Description = "Block a user in the system";
        Response(200, "Users was successfully Blocked");
    }
}