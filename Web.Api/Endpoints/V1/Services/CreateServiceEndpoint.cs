using FastEndpoints;
using Managements.Application.Services;
using Mediator;

namespace Web.Api.Endpoints.V1.Services;

internal sealed class CreateServiceEndpoint : Endpoint<CreateServiceCommand>
{
    private readonly IMediator _mediator;

    public CreateServiceEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("services");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateServiceCommand req, CancellationToken ct)
    {
        var service = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<ServiceEndpoint>(new {service.Id}, new ServiceResponse
            {
                Id = service.Id.Value,
                Name = service.Name,
                Secret = service.Secret.ToString(),
                Status = service.Status.ToString()
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class CreateServiceEndpointSummary : Summary<CreateServiceEndpoint>
{
    public CreateServiceEndpointSummary()
    {
        Summary = "Create a new Service in the system";
        Description = "Create a new Service in the system";
        Response(201, "Service was successfully created");
    }
}