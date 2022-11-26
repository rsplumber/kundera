using Core.Domains.Credentials;
using Core.Domains.Users.Types;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Endpoints.V1.Auth.Credentials;

internal sealed class CreateCredentialEndpoint : Endpoint<CreateCredentialRequest>
{
    private readonly ICredentialService _credentialService;

    public CreateCredentialEndpoint(ICredentialService credentialService)
    {
        _credentialService = credentialService;
    }


    public override void Configure()
    {
        Post("users/{UserId:guid}/credentials");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateCredentialRequest req, CancellationToken ct)
    {
        var uniqueIdentifier = UniqueIdentifier.From(req.Username, req.Type);
        await _credentialService.CreateAsync(uniqueIdentifier,
            req.Password,
            UserId.From(req.UserId),
            HttpContext.Connection.LocalIpAddress,
            ct);

        await SendOkAsync(ct);
    }
}

internal sealed class CreateCredentialEndpointSummary : Summary<CreateCredentialEndpoint>
{
    public CreateCredentialEndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(200, "Session terminated successfully");
    }
}

public sealed record CreateCredentialRequest
{
    public Guid UserId { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? Type { get; set; }
}

internal sealed class CreateCredentialRequestValidator : AbstractValidator<CreateCredentialRequest>
{
    public CreateCredentialRequestValidator()
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