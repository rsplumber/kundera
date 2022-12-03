using Application.Groups;
using Application.Roles;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles;

internal sealed class RolePermissionsEndpoint : Endpoint<RolePermissionsQuery, List<RolePermissionsResponse>>
{
    private readonly IMediator _mediator;

    public RolePermissionsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("roles/{id:guid}/permissions");
        Permissions("roles_permissions_list");
        Version(1);
    }

    public override async Task HandleAsync(RolePermissionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class RolePermissionsEndpointSummary : Summary<RolePermissionsEndpoint>
{
    public RolePermissionsEndpointSummary()
    {
        Summary = "Role permissions list";
        Description = "Roles permissions list";
        Response<GroupResponse>(200, "Role permissions was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}