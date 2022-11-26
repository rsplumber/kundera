using FastEndpoints;
using Managements.Application.Scopes;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class ActivateScopeEndpoint : Endpoint<ActivateScopeCommand>
{
    private readonly IMediator _mediator;

    public ActivateScopeEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{id:guid}/active");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(ActivateScopeCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class ActivateScopeEndpointSummary : Summary<ActivateScopeEndpoint>
{
    public ActivateScopeEndpointSummary()
    {
        Summary = "Activate a scope in the system";
        Description = "Activate a scope in the system";
        Response(200, "Scopes was successfully Activated");
    }
}