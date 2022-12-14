using Application.Scopes;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes.Services.Add;

internal sealed class Endpoint : Endpoint<AddScopeServiceCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{scopeId:guid}/services");
        Permissions("scopes_add_service");
        Version(1);
    }

    public override async Task HandleAsync(AddScopeServiceCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Add scope service in the system";
        Description = "Add scope service in the system";
        Response(200, "Scope service was successfully added");
    }
}