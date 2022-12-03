using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class RemoveUserFromGroupEndpoint : Endpoint<RemoveUserFromGroupCommand>
{
    private readonly IMediator _mediator;

    public RemoveUserFromGroupEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{id:guid}/groups");
        Permissions("user_remove_group");
        Version(1);
    }

    public override async Task HandleAsync(RemoveUserFromGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class RemoveUserFromGroupEndpointSummary : Summary<RemoveUserFromGroupEndpoint>
{
    public RemoveUserFromGroupEndpointSummary()
    {
        Summary = "Remove a user from a group role in the system";
        Description = "Remove a user from a group in the system";
        Response(204, "User was successfully removed from the group");
    }
}