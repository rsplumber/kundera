using System.Net;
using Data.Abstractions.Services;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Services.Create;

file sealed class Endpoint : Endpoint<CreateServiceCommand>
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
        await SendAsync(new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Secret = service.Secret,
            Status = service.Status.ToString()
        }, (int)HttpStatusCode.Created, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new Service in the system";
        Description = "Create a new Service in the system";
        Response(201, "Service was successfully created");
    }
}

file sealed class RequestValidator : Validator<CreateServiceCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter a Name")
            .NotNull().WithMessage("Enter a Name");
    }
}