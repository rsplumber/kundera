using Application.Scopes;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class DeleteScopeEndpoint : Endpoint<DeleteScopeCommand>
{
    private readonly IMediator _mediator;

    public DeleteScopeEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("scopes/{scopeId:guid}");
        Permissions("scopes_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteScopeCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteScopeEndpointSummary : Summary<DeleteScopeEndpoint>
{
    public DeleteScopeEndpointSummary()
    {
        Summary = "Delete a scope in the system";
        Description = "Delete a scope in the system";
        Response(204, "Scope was successfully deleted");
    }
}