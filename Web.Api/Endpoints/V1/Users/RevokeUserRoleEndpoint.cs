using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users;

internal sealed class RevokeUserRoleEndpoint : Endpoint<RevokeUserRoleCommand>
{
    private readonly IMediator _mediator;

    public RevokeUserRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{id:guid}/roles");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RevokeUserRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class RevokeUserRoleEndpointSSummary : Summary<RevokeUserRoleEndpoint>
{
    public RevokeUserRoleEndpointSSummary()
    {
        Summary = "Revoke a role from a user in the system";
        Description = "Revoke a role from a user a in the system";
        Response(204, "Role was successfully revoked from the user");
    }
}