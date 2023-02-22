using Commands.Services;
using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Services;

namespace Web.Endpoints.V1.Services.Create;

internal sealed class Endpoint : Endpoint<CreateServiceCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("services");
        Permissions("services_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateServiceCommand req, CancellationToken ct)
    {
        var service = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<Details.Endpoint>(new { service.Id }, new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Secret = service.Secret,
                Status = service.Status.ToString()
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class CreateServiceEndpointSummary : Summary<Endpoint>
{
    public CreateServiceEndpointSummary()
    {
        Summary = "Create a new Service in the system";
        Description = "Create a new Service in the system";
        Response(201, "Service was successfully created");
    }
}

internal sealed class RequestValidator : Validator<CreateServiceCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter a Name")
            .NotNull().WithMessage("Enter a Name");
    }
}