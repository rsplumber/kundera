using Application.Scopes;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class AddScopeRoleEndpoint : Endpoint<AddScopeRoleCommand>
{
    private readonly IMediator _mediator;

    public AddScopeRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("scopes/{scopeId:guid}/roles");
        Permissions("scopes_add_role");
        Version(1);
    }

    public override async Task HandleAsync(AddScopeRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class AddScopeRoleEndpointSummary : Summary<AddScopeRoleEndpoint>
{
    public AddScopeRoleEndpointSummary()
    {
        Summary = "Add scope role in the system";
        Description = "Add scope role in the system";
        Response(200, "Scope role was successfully added");
    }
}