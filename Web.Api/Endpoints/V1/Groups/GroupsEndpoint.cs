using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class GroupsEndpoint : EndpointWithoutRequest<List<GroupsResponse>>
{
    private readonly IMediator _mediator;

    public GroupsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("groups");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GroupsQuery();

        var response = await _mediator.Send(query, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class GroupsEndpointSummary : Summary<GroupsEndpoint>
{
    public GroupsEndpointSummary()
    {
        Summary = "Groups list";
        Description = "Groups list";
        Response<IEnumerable<GroupResponse>>(200, "Groups was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}