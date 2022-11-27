using Application.Permissions;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Permissions;

internal sealed class DeletePermissionEndpoint : Endpoint<DeletePermissionCommand>
{
    private readonly IMediator _mediator;

    public DeletePermissionEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("permissions/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(DeletePermissionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeletePermissionEndpointSummary : Summary<DeletePermissionEndpoint>
{
    public DeletePermissionEndpointSummary()
    {
        Summary = "Delete a permission in the system";
        Description = "Delete a permission in the system";
        Response(204, "Permission was successfully deleted");
    }
}