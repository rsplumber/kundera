using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Services.Status.DeActivate;

file sealed class Endpoint : Endpoint<DeActivateServiceCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("services/{serviceId:guid}/de-activate");
        Permissions("services_de-activate");
        Version(1);
    }

    public override async Task HandleAsync(DeActivateServiceCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Activate a Service in the system";
        Description = "Activate a new Service in the system";
        Response(200, "Service was successfully activated");
    }
}

file sealed class RequestValidator : Validator<DeActivateServiceCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ServiceId)
            .NotEmpty().WithMessage("Enter a ServiceId")
            .NotNull().WithMessage("Enter a ServiceId");
    }
}