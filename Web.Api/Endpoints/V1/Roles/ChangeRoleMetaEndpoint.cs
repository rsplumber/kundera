using Application.Roles;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles;

internal sealed class ChangeRoleMetaEndpoint : Endpoint<ChangeRoleMetaCommand>
{
    private readonly IMediator _mediator;

    public ChangeRoleMetaEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("roles/{roleId:guid}/meta");
        Permissions("roles_change_meta");
        Version(1);
    }

    public override async Task HandleAsync(ChangeRoleMetaCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class ChangeRoleMetaEndpointSummary : Summary<ChangeRoleMetaEndpoint>
{
    public ChangeRoleMetaEndpointSummary()
    {
        Summary = "Change role meta in the system";
        Description = "Change role meta in the system";
        Response(200, "Role meta was successfully changed");
    }
}