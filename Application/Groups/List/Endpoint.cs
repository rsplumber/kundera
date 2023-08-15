using Data.Abstractions;
using Data.Abstractions.Groups;
using FastEndpoints;
using Mediator;

namespace Application.Groups.List;

file sealed class Endpoint : EndpointWithoutRequest<PageableResponse<GroupsResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("groups");
        Permissions("groups_list");
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GroupsQuery();
        var response = await _mediator.Send(query, ct);
        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Groups list";
        Description = "Groups list";
        Response<PageableResponse<GroupResponse>>(200, "Groups was successfully received");
    }
}