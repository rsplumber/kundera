using FastEndpoints;
using Mediator;
using Queries;
using Queries.Groups;
using Queries.Services;

namespace Application.Services.List;

file sealed class Endpoint : Endpoint<ServicesQuery, PageableResponse<ServicesResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("services");
        Permissions("services_list");
        Version(1);
    }

    public override async Task HandleAsync(ServicesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Services list";
        Description = "Services list";
        Response<PageableResponse<GroupResponse>>(200, "Services was successfully received");
    }
}