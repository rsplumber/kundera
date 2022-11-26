using Application.Permissions;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Permissions;

internal sealed class ChangePermissionMetaEndpoint : Endpoint<ChangePermissionMetaCommand>
{
    private readonly IMediator _mediator;

    public ChangePermissionMetaEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("permissions/{id:guid}/meta");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(ChangePermissionMetaCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class ChangePermissionMetaEndpointSummary : Summary<ChangePermissionMetaEndpoint>
{
    public ChangePermissionMetaEndpointSummary()
    {
        Summary = "Change permission meta in the system";
        Description = "Change permission meta in the system";
        Response(200, "Permission meta was successfully changed");
    }
}