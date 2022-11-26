using FastEndpoints;
using Managements.Application.Users;
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
        Post("users/{id:guid}/usernames");
        AllowAnonymous();
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