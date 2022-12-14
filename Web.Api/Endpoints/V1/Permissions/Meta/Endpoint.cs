using Application.Permissions;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Permissions.Meta;

internal sealed class Endpoint : Endpoint<ChangePermissionMetaCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("permissions/{permissionId:guid}/meta");
        AllowAnonymous();
        Permissions("permissions_change_meta");
        Version(1);
    }

    public override async Task HandleAsync(ChangePermissionMetaCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Change permission meta in the system";
        Description = "Change permission meta in the system";
        Response(200, "Permission meta was successfully changed");
    }
}