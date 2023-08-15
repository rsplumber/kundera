using Data.Abstractions;
using Data.Abstractions.Groups;
using Data.Abstractions.Scopes;
using FastEndpoints;
using Mediator;

namespace Application.Scopes.List;

file sealed class Endpoint : Endpoint<ScopesQuery, PageableResponse<ScopesResponse>>
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

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Scopes list";
        Description = "Scopes list";
        Response<PageableResponse<GroupResponse>>(200, "Scopes was successfully received");
    }
}