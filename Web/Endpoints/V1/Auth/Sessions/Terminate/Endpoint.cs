using Commands.Auth.Sessions;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Auth.Sessions.Terminate;

internal sealed class Endpoint : Endpoint<TerminateSessionCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }


    public override void Configure()
    {
        Delete("sessions/terminate");
        Permissions("kundera_sessions_terminate");
        Version(1);
    }

    public override async Task HandleAsync(TerminateSessionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(204, "Session terminated successfully");
    }
}

internal sealed class RequestValidator : Validator<TerminateSessionCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Enter valid Id")
            .NotNull().WithMessage("Enter valid Id");
    }
}