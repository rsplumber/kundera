using Application.Groups;
using Application.Permissions;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Permissions;

internal sealed class PermissionEndpoint : Endpoint<PermissionQuery, PermissionResponse>
{
    private readonly IMediator _mediator;

    public PermissionEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("permissions/{permissionId:guid}");
        Permissions("permissions_get");
        Version(1);
    }

    public override async Task HandleAsync(PermissionQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class PermissionEndpointSummary : Summary<PermissionEndpoint>
{
    public PermissionEndpointSummary()
    {
        Summary = "Permission details";
        Description = "Permission details";
        Response<GroupResponse>(200, "Permission was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}