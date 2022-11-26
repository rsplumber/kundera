using FastEndpoints;
using Managements.Application.Groups;
using Managements.Application.Permissions;
using Managements.Application.Services;
using Mediator;

namespace Web.Api.Endpoints.V1.Services;

internal sealed class ServiceEndpoint : Endpoint<ServiceQuery, ServiceResponse>
{
    private readonly IMediator _mediator;

    public ServiceEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("services/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(ServiceQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class ServiceEndpointSummary : Summary<ServiceEndpoint>
{
    public ServiceEndpointSummary()
    {
        Summary = "Service details";
        Description = "Service details";
        Response<GroupResponse>(200, "Service was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}