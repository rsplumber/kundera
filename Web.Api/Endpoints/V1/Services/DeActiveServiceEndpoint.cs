using Application.Services;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Services;

internal sealed class DeActiveServiceEndpoint : Endpoint<ActivateServiceCommandValidator>
{
    private readonly IMediator _mediator;

    public DeActiveServiceEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("services/{id:guid}/de-activate");
        Permissions("services_de-activate");
        Version(1);
    }

    public override async Task HandleAsync(ActivateServiceCommandValidator req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class DeActiveServiceEndpointSummary : Summary<DeActiveServiceEndpoint>
{
    public DeActiveServiceEndpointSummary()
    {
        Summary = "Activate a Service in the system";
        Description = "Activate a new Service in the system";
        Response(200, "Service was successfully activated");
    }
}