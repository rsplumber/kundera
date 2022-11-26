using FastEndpoints;
using Managements.Application.Scopes;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class AddScopeServiceEndpoint : Endpoint<AddScopeServiceCommand>
{
    private readonly IMediator _mediator;

    public AddScopeServiceEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{id:guid}/services");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(AddScopeServiceCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class AddScopeServiceEndpointSummary : Summary<AddScopeServiceEndpoint>
{
    public AddScopeServiceEndpointSummary()
    {
        Summary = "Add scope service in the system";
        Description = "Add scope service in the system";
        Response(200, "Scope service was successfully added");
    }
}