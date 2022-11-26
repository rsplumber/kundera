using Application.Groups;
using Application.Services;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Services;

internal sealed class ServicesEndpoint : Endpoint<ServicesQuery, List<ServicesResponse>>
{
    private readonly IMediator _mediator;

    public ServicesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("services");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(ServicesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class ServicesEndpointSummary : Summary<ServicesEndpoint>
{
    public ServicesEndpointSummary()
    {
        Summary = "Services list";
        Description = "Services list";
        Response<GroupResponse>(200, "Services was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}