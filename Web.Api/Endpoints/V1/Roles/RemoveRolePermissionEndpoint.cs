using Application.Roles;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles;

internal sealed class RemoveRolePermissionEndpoint : Endpoint<RemoveRolePermissionCommand>
{
    private readonly IMediator _mediator;

    public RemoveRolePermissionEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("roles/{id:guid}/permission");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RemoveRolePermissionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class RemoveRolePermissionEndpointSummary : Summary<RemoveRolePermissionEndpoint>
{
    public RemoveRolePermissionEndpointSummary()
    {
        Summary = "Remove role permission in the system";
        Description = "Remove role permission in the system";
        Response(204, "Role permission was successfully removed");
    }
}