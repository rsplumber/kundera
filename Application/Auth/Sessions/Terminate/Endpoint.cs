using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Auth.Sessions.Terminate;

file sealed class Endpoint : Endpoint<TerminateSessionCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }


    public override void Configure()
    {
        Delete("sessions/terminate");
        Permissions("sessions_terminate");
        Version(1);
    }

    public override async Task HandleAsync(TerminateSessionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(204, "Session terminated successfully");
    }
}

file sealed class RequestValidator : Validator<TerminateSessionCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Enter valid Token")
            .NotNull().WithMessage("Enter valid Token");
    }
}