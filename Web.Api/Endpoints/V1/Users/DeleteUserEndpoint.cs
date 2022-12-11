using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class DeleteUserEndpoint : Endpoint<DeleteUserCommand>
{
    private readonly IMediator _mediator;

    public DeleteUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{userId:guid}");
        Permissions("user_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteUserEndpointSummary : Summary<DeleteUserEndpoint>
{
    public DeleteUserEndpointSummary()
    {
        Summary = "Delete a user in the system";
        Description = "Delete a user in the system";
        Response(204, "User was successfully deleted");
    }
}