using Core.Domains.Credentials;
using Core.Domains.Users.Types;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Endpoints.V1.Auth.Credentials;

internal sealed class CreateTimePeriodicEndpoint : Endpoint<CreateTimePeriodicCredentialRequest>
{
    private readonly ICredentialService _credentialService;

    public CreateTimePeriodicEndpoint(ICredentialService credentialService)
    {
        _credentialService = credentialService;
    }


    public override void Configure()
    {
        Post("users/{UserId:guid}/credentials/time-periodic");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateTimePeriodicCredentialRequest req, CancellationToken ct)
    {
        var uniqueIdentifier = UniqueIdentifier.From(req.Username, req.Type);
        await _credentialService.CreateTimePeriodicAsync(uniqueIdentifier,
            req.Password,
            UserId.From(req.UserId),
            req.ExpireInMinutes,
            HttpContext.Connection.LocalIpAddress,
            ct);

        await SendOkAsync(ct);
    }
}

internal sealed class CreateTimePeriodicEndpointSummary : Summary<CreateTimePeriodicEndpoint>
{
    public CreateTimePeriodicEndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(200, "Session terminated successfully");
    }
}

public record CreateTimePeriodicCredentialRequest
{
    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public int ExpireInMinutes { get; init; } = default!;

    public string? Type { get; init; }
}

public class CreateTimePeriodicCredentialRequestValidator : AbstractValidator<CreateTimePeriodicCredentialRequest>
{
    public CreateTimePeriodicCredentialRequestValidator()
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
            .LessThanOrEqualTo(0).WithMessage("Enter valid ExpireInMinutes");
    }
}