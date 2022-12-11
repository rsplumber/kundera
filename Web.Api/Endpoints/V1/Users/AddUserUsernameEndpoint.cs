using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class AddUserUsernameEndpoint : Endpoint<AddUserUsernameCommand>
{
    private readonly IMediator _mediator;

    public AddUserUsernameEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/usernames");
        Permissions("user_add_username");
        Version(1);
    }

    public override async Task HandleAsync(AddUserUsernameCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class AddUserUsernameEndpointSummary : Summary<AddUserUsernameEndpoint>
{
    public AddUserUsernameEndpointSummary()
    {
        Summary = "Add user username in the system";
        Description = "Add user username in the system";
        Response(200, "User username was successfully added");
    }
}