using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups.Role.Assign;

internal sealed class Endpoint : Endpoint<AssignGroupRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{groupId:guid}/roles");
        Permissions("groups_assign_role");
        Version(1);
    }

    public override async Task HandleAsync(AssignGroupRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Assign a role to a group in the system";
        Description = "Assign a role to a group in the system";
        Response(200, "Role was successfully assigned to the group");
    }
}