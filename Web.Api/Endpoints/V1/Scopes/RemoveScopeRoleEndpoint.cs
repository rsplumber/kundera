using FastEndpoints;
using Managements.Application.Scopes;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class RemoveScopeRoleEndpoint : Endpoint<RemoveScopeRoleCommand>
{
    private readonly IMediator _mediator;

    public RemoveScopeRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("scopes/{id:guid}/roles");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RemoveScopeRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class RemoveScopeRoleEndpointSummary : Summary<RemoveScopeRoleEndpoint>
{
    public RemoveScopeRoleEndpointSummary()
    {
        Summary = "Remove scope role in the system";
        Description = "Remove scope role in the system";
        Response(204, "Scope role was successfully removed");
    }
}