using Application.Auth.Credentials;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Credentials.TimePeriodic;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/credentials/time-periodic");
        Permissions("credentials_create_time-periodic");
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = new CreateTimePeriodicCredentialCommand
        {
            Username = req.Username,
            Password = req.Password,
            Type = req.Type,
            ExpireInMinutes = req.ExpireInMinutes,
            IpAddress = HttpContext.Connection.RemoteIpAddress,
            UserId = req.UserId
        };

        await _mediator.Send(command, ct);
        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create TimePeriodic Credential";
        Description = "Create a TimePeriodic Credential that expires after period of time";
        Response(200, "Credential Created successfully");
    }
}

internal sealed record Request
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; } = default!;

    public string? Type { get; init; } = default;
}

internal sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter valid UserId")
            .NotNull().WithMessage("Enter valid UserId");

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