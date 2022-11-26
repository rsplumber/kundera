using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class JoinUserToGroupEndpoint : Endpoint<JoinUserToGroupCommand>
{
    private readonly IMediator _mediator;

    public JoinUserToGroupEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{id:guid}/groups");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(JoinUserToGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class JoinUserToGroupEndpointSummary : Summary<JoinUserToGroupEndpoint>
{
    public JoinUserToGroupEndpointSummary()
    {
        Summary = "Joint a user to a group role in the system";
        Description = "Joint a user to a group in the system";
        Response(200, "User was successfully joined to the group");
    }
}