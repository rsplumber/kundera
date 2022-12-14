using Application.Groups;
using Application.Services;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Services.Details;

internal sealed class Endpoint : Endpoint<ServiceQuery, ServiceResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("services/{serviceId:guid}");
        Permissions("services_get");
        Version(1);
    }

    public override async Task HandleAsync(ServiceQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Service details";
        Description = "Service details";
        Response<GroupResponse>(200, "Service was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}