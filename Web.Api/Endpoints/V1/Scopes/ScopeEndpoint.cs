using FastEndpoints;
using Managements.Application.Groups;
using Managements.Application.Scopes;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class ScopeEndpoint : Endpoint<ScopeQuery, ScopeResponse>
{
    private readonly IMediator _mediator;

    public ScopeEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("scopes/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(ScopeQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class ScopeEndpointSummary : Summary<ScopeEndpoint>
{
    public ScopeEndpointSummary()
    {
        Summary = "Scope details";
        Description = "Scope details";
        Response<GroupResponse>(200, "Scope was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}