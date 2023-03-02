using Application.Services;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Services.Status.DeActivate;

internal sealed class Endpoint : Endpoint<DeActivateServiceCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("services/{serviceId:guid}/de-activate");
        Permissions("kundera_services_de-activate");
        Version(1);
    }

    public override async Task HandleAsync(DeActivateServiceCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Activate a Service in the system";
        Description = "Activate a new Service in the system";
        Response(200, "Service was successfully activated");
    }
}

internal sealed class RequestValidator : Validator<DeActivateServiceCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ServiceId)
            .NotEmpty().WithMessage("Enter a ServiceId")
            .NotNull().WithMessage("Enter a ServiceId");
    }
}