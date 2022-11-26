using FastEndpoints;
using Managements.Application.Groups;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class RevokeGroupRoleEndpoint : Endpoint<RevokeGroupRoleCommand>
{
    private readonly IMediator _mediator;

    public RevokeGroupRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("groups/{id:guid}/roles");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RevokeGroupRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class RevokeGroupRoleEndpointSummary : Summary<RevokeGroupRoleEndpoint>
{
    public RevokeGroupRoleEndpointSummary()
    {
        Summary = "Revoke a role from a group in the system";
        Description = "Revoke a role from a group a in the system";
        Response(204, "Role was successfully revoked from the user");
    }
}