using Application.Users;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Users.Roles.Assign;

internal sealed class RoleEndpoint : Endpoint<AssignUserRoleCommand>
{
    private readonly IMediator _mediator;

    public RoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/roles");
        Permissions("user_assign_role");
        Version(1);
    }

    public override async Task HandleAsync(AssignUserRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSSummary : Summary<RoleEndpoint>
{
    public EndpointSSummary()
    {
        Summary = "Assign user role in the system";
        Description = "Assign user role in the system";
        Response(200, "User role was successfully assigned");
    }
}