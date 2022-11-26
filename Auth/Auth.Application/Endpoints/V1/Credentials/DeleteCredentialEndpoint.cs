using Core.Domains.Credentials;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Auth.Application.Endpoints.V1.Credentials;

internal sealed class DeleteCredentialEndpoint : Endpoint<DeleteCredentialRequest>
{
    private readonly ICredentialService _credentialService;

    public DeleteCredentialEndpoint(ICredentialService credentialService)
    {
        _credentialService = credentialService;
    }

    public override void Configure()
    {
        Delete("credentials/{uniqueIdentifier}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(DeleteCredentialRequest req, CancellationToken ct)
    {
        await _credentialService.RemoveAsync(UniqueIdentifier.Parse(req.UniqueIdentifier), ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteCredentialEndpointSummary : Summary<DeleteCredentialEndpoint>
{
    public DeleteCredentialEndpointSummary()
    {
        Summary = "Terminate session";
        Description = "Terminate a session";
        Response(200, "Session terminated successfully");
    }
}

public sealed record DeleteCredentialRequest
{
    public string UniqueIdentifier { get; set; } = default!;
}

internal sealed class DeleteCredentialRequestValidator : AbstractValidator<DeleteCredentialRequest>
{
    public DeleteCredentialRequestValidator()
    {
        RuleFor(request => request.UniqueIdentifier)
            .NotEmpty().WithMessage("Enter valid UniqueIdentifier")
            .NotNull().WithMessage("Enter valid UniqueIdentifier");
    }
}