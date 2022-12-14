using Application.Scopes;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes.Roles.Delete;

internal sealed class Endpoint : Endpoint<RemoveScopeRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("scopes/{scopeId:guid}/roles");
        Permissions("scopes_remove_role");
        Version(1);
    }

    public override async Task HandleAsync(RemoveScopeRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove scope role in the system";
        Description = "Remove scope role in the system";
        Response(204, "Scope role was successfully removed");
    }
}