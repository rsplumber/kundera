using FastEndpoints;
using Managements.Application.Scopes;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class DeActivateScopeEndpoint : Endpoint<DeActivateScopeCommand>
{
    private readonly IMediator _mediator;

    public DeActivateScopeEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{id:guid}/de-active");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(DeActivateScopeCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class DeActivateScopeEndpointSummary : Summary<DeActivateScopeEndpoint>
{
    public DeActivateScopeEndpointSummary()
    {
        Summary = "DeActivate a scope in the system";
        Description = "DeActivate a scope in the system";
        Response(200, "Scopes was successfully DeActivated");
    }
}