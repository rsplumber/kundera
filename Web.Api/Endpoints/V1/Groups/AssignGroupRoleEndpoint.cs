using FastEndpoints;
using Managements.Application.Groups;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class AssignGroupRoleEndpoint : Endpoint<AssignGroupRoleCommand>
{
    private readonly IMediator _mediator;

    public AssignGroupRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{id:guid}/roles");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(AssignGroupRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class AssignGroupRoleEndpointSummary : Summary<AssignGroupRoleEndpoint>
{
    public AssignGroupRoleEndpointSummary()
    {
        Summary = "Assign a role to a group in the system";
        Description = "Assign a role to a group in the system";
        Response(200, "Role was successfully assigned to the group");
    }
}