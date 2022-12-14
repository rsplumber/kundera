using Application.Services;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Services.Delete;

internal sealed class Endpoint : Endpoint<DeleteServiceCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("services/{serviceId:guid}");
        Permissions("services_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteServiceCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Delete a service in the system";
        Description = "Delete a service in the system";
        Response(204, "Service was successfully deleted");
    }
}