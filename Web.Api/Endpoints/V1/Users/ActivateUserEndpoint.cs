using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class ActivateUserEndpoint : Endpoint<ActiveUserCommand>
{
    private readonly IMediator _mediator;

    public ActivateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("user/{id:guid}/activate");
        Permissions("user_activate");
        Version(1);
    }

    public override async Task HandleAsync(ActiveUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class ActivateUserEndpointSummary : Summary<ActivateUserEndpoint>
{
    public ActivateUserEndpointSummary()
    {
        Summary = "Activate a user in the system";
        Description = "Activate a user in the system";
        Response(200, "Users was successfully Activated");
    }
}