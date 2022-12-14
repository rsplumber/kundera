using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups.Details;

internal sealed class Endpoint : Endpoint<GroupQuery, GroupResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("groups/{GroupId:guid}");
        Permissions("groups_get");
        Version(1);
    }

    public override async Task HandleAsync(GroupQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Group details";
        Description = "Group details";
        Response<GroupResponse>(200, "Group was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}