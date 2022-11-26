using Application.Roles;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles;

internal sealed class DeleteRoleEndpoint : Endpoint<DeleteRoleCommand>
{
    private readonly IMediator _mediator;

    public DeleteRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("roles/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(DeleteRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteRoleEndpointSummary : Summary<CreateRoleEndpoint>
{
    public DeleteRoleEndpointSummary()
    {
        Summary = "Delete a role in the system";
        Description = "Delete a role in the system";
        Response(204, "Role was successfully deleted");
    }
}