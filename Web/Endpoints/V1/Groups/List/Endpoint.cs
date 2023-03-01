using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Endpoints.V1.Groups.List;

internal sealed class Endpoint : EndpointWithoutRequest<List<GroupsResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("groups");
        Permissions("kundera_groups_list");
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GroupsQuery();
        var response = await _mediator.Send(query, ct);
        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Groups list";
        Description = "Groups list";
        Response<List<GroupResponse>>(200, "Groups was successfully received");
    }
}