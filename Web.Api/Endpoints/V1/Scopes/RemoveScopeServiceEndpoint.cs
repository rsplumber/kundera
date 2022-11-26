using Application.Scopes;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class RemoveScopeServiceEndpoint : Endpoint<RemoveScopeServiceCommand>
{
    private readonly IMediator _mediator;

    public RemoveScopeServiceEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("scopes/{id:guid}/services");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RemoveScopeServiceCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class RemoveScopeServiceEndpointSummary : Summary<RemoveScopeServiceEndpoint>
{
    public RemoveScopeServiceEndpointSummary()
    {
        Summary = "Remove scope service in the system";
        Description = "Remove scope service in the system";
        Response(204, "Scope service was successfully removed");
    }
}