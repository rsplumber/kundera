using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Auth.Credentials.Basic;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{userId:guid}/credentials");
        Permissions("credentials_create");
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = new CreateBasicCredentialCommand
        {
            Username = req.Username,
            Password = req.Password,
            UserId = req.UserId,
            SingleSession = req.SingleSession,
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
        Summary = "Create Credential";
        Description = "Create a default Credential";
        Response(200, "Credential Created successfully");
    }
}

internal sealed record Request
{
    public Guid UserId { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public bool? SingleSession { get; set; }

    public int SessionExpireTimeInMinutes { get; set; }
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

        RuleFor(request => request.SessionExpireTimeInMinutes)
            .NotEmpty().WithMessage("Enter valid SessionExpireTimeInMinutes")
            .NotNull().WithMessage("Enter valid SessionExpireTimeInMinutes")
            .GreaterThan(1).WithMessage("SessionExpireTimeInMinutes must be greater than 1");
    }
}