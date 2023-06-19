using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Services.Delete;

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

internal sealed class RequestValidator : Validator<DeleteServiceCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ServiceId)
            .NotEmpty().WithMessage("Enter a ServiceId")
            .NotNull().WithMessage("Enter a ServiceId");
    }
}