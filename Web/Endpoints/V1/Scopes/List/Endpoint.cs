using FastEndpoints;
using Mediator;
using Queries.Groups;
using Queries.Scopes;

namespace Web.Endpoints.V1.Scopes.List;

internal sealed class Endpoint : Endpoint<ScopesQuery, List<ScopesResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("scopes");
        Permissions("scopes_list");
        Version(1);
    }

    public override async Task HandleAsync(ScopesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Scopes list";
        Description = "Scopes list";
        Response<GroupResponse>(200, "Scopes was successfully received");
    }
}