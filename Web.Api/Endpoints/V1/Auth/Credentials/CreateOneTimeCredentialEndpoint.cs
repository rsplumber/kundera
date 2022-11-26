using Core.Domains.Credentials;
using Core.Domains.Users.Types;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Endpoints.V1.Auth.Credentials;

internal sealed class CreateOneTimeCredentialEndpoint : Endpoint<CreateOneTimeCredentialRequest>
{
    private readonly ICredentialService _credentialService;

    public CreateOneTimeCredentialEndpoint(ICredentialService credentialService)
    {
        _credentialService = credentialService;
    }


    public override void Configure()
    {
        Post("users/{UserId:guid}/credentials/one-time");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateOneTimeCredentialRequest req, CancellationToken ct)
    {
        var uniqueIdentifier = UniqueIdentifier.From(req.Username, req.Type);
        await _credentialService.CreateOneTimeAsync(uniqueIdentifier,
            req.Password,
            UserId.From(req.UserId),
            req.ExpireInMinutes,
            HttpContext.Connection.LocalIpAddress,
            ct);
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