using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class RemoveUserUsernameEndpoint : Endpoint<RemoveUserUsernameCommand>
{
    private readonly IMediator _mediator;

    public RemoveUserUsernameEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{id:guid}/usernames");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RemoveUserUsernameCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class RemoveUserUsernameEndpointSSummary : Summary<RemoveUserUsernameEndpoint>
{
    public RemoveUserUsernameEndpointSSummary()
    {
        Summary = "Remove user username in the system";
        Description = "Remove user username in the system";
        Response(204, "User username was successfully removed");
    }
}