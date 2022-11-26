using FastEndpoints;
using Managements.Application.Services;
using Mediator;

namespace Web.Api.Endpoints.V1.Services;

internal sealed class ActiveServiceEndpoint : Endpoint<ActivateServiceCommandValidator>
{
    private readonly IMediator _mediator;

    public ActiveServiceEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("services/{id:guid}/activate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(ActivateServiceCommandValidator req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class ActiveServiceEndpointSummary : Summary<ActiveServiceEndpoint>
{
    public ActiveServiceEndpointSummary()
    {
        Summary = "Activate a Service in the system";
        Description = "Activate a new Service in the system";
        Response(200, "Service was successfully activated");
    }
}