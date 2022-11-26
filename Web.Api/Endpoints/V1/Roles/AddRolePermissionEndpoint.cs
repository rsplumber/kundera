using FastEndpoints;
using Managements.Application.Roles;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles;

internal sealed class AddRolePermissionEndpoint : Endpoint<AddRolePermissionCommand>
{
    private readonly IMediator _mediator;

    public AddRolePermissionEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("roles/{id:guid}/permissions");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(AddRolePermissionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class AddRolePermissionEndpointSummary : Summary<AddRolePermissionEndpoint>
{
    public AddRolePermissionEndpointSummary()
    {
        Summary = "Add role permission in the system";
        Description = "Add role permission in the system";
        Response(200, "Role permission was successfully added");
    }
}