using Application.Groups;
using Application.Scopes;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes.Roles.List;

internal sealed class Endpoint : Endpoint<ScopeRolesQuery, List<ScopeRolesResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("scopes/{scopeId:guid}/roles");
        Permissions("scopes_roles_list");
        Version(1);
    }

    public override async Task HandleAsync(ScopeRolesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Scope roles list";
        Description = "Scope roles list";
        Response<GroupResponse>(200, "Scope roles was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}