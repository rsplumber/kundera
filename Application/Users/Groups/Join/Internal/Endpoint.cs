using FastEndpoints;
using Mediator;

namespace Application.Users.Groups.Join.Internal;

file sealed class Endpoint : Endpoint<JoinUserToGroupCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("internal/users/{userId:guid}/groups");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(JoinUserToGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Joint a user to a group role in the system";
        Description = "Joint a user to a group in the system";
        Response(200, "User was successfully joined to the group");
    }
}