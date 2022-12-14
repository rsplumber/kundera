using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users.Groups.Left;

internal sealed class Endpoint : Endpoint<RemoveUserFromGroupCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{userId:guid}/groups");
        Permissions("user_remove_group");
        Version(1);
    }

    public override async Task HandleAsync(RemoveUserFromGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove a user from a group role in the system";
        Description = "Remove a user from a group in the system";
        Response(204, "User was successfully removed from the group");
    }
}