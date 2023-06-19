using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials.OneTime;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/credentials/one-time");
        Permissions("credentials_create_onetime");
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = new CreateOneTimeCredentialCommand
        {
            Username = req.Username,
            Password = req.Password,
            ExpireInMinutes = req.ExpireInMinutes,
            UserId = req.UserId,
            SessionExpireTimeInMinutes = req.SessionExpireTimeInMinutes
        };

        await _mediator.Send(command, ct);
        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create OneTime Credential";
        Description = "Create a OneTime Credential that expires after one use";
        Response(200, "Credential Created successfully");
    }
}

internal sealed record Request
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; }

    public int SessionExpireTimeInMinutes { get; init; }
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

        RuleFor(request => request.SessionExpireTimeInMinutes)
            .NotEmpty().WithMessage("Enter valid SessionExpireTimeInMinutes")
            .NotNull().WithMessage("Enter valid SessionExpireTimeInMinutes")
            .GreaterThan(1).WithMessage("SessionExpireTimeInMinutes must be greater than 1");
    }
}