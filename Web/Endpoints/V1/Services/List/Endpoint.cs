using FastEndpoints;
using Mediator;
using Queries.Groups;
using Queries.Services;

namespace Web.Endpoints.V1.Services.List;

internal sealed class Endpoint : Endpoint<ServicesQuery, List<ServicesResponse>>
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

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Services list";
        Description = "Services list";
        Response<GroupResponse>(200, "Services was successfully received");
    }
}