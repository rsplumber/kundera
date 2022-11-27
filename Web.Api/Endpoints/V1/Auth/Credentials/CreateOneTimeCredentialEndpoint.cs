using Application.Auth.Credentials;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Api.Endpoints.V1.Auth.Credentials;

internal sealed class CreateOneTimeCredentialEndpoint : Endpoint<CreateOneTimeCredentialRequest>
{
    private readonly IMediator _mediator;

    public CreateOneTimeCredentialEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users/{UserId:guid}/credentials/one-time");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateOneTimeCredentialRequest req, CancellationToken ct)
    {
        var command = new CreateOneTimeCredentialCommand
        {
            Username = req.Username,
            Password = req.Password,
            Type = req.Type,
            ExpireInMinutes = req.ExpireInMinutes,
            IpAddress = HttpContext.Connection.LocalIpAddress,
            UserId = req.UserId
        };

        await _mediator.Send(command, ct);
        await SendOkAsync(ct);
    }
}

internal sealed class CreateOneTimeCredentialEndpointSummary : Summary<CreateOneTimeCredentialEndpoint>
{
    public CreateOneTimeCredentialEndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(200, "Session terminated successfully");
    }
}

public record CreateOneTimeCredentialRequest
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string? Type { get; init; }

    public int ExpireInMinutes { get; init; } = 0;
}

public class CreateOneTimeCredentialRequestValidator : AbstractValidator<CreateOneTimeCredentialRequest>
{
    public CreateOneTimeCredentialRequestValidator()
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
    }
}