using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials.TimePeriodic;

file sealed class Endpoint : Endpoint<CreateTimePeriodicCredentialCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/credentials/time-periodic");
        Permissions("credentials_create_time-periodic");
        Version(1);
    }

    public override async Task HandleAsync(CreateTimePeriodicCredentialCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create TimePeriodic Credential";
        Description = "Create a TimePeriodic Credential that expires after period of time";
        Response(200, "Credential Created successfully");
    }
}

file sealed class RequestValidator : Validator<CreateTimePeriodicCredentialCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");

        RuleFor(request => request.ExpireInMinutes)
            .NotEmpty().WithMessage("Enter valid ExpireInMinutes")
            .NotNull().WithMessage("Enter valid ExpireInMinutes")
            .GreaterThan(1).WithMessage("ExpireInMinutes must be greater than 1");
    }
}