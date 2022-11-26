using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Auth.Application.Endpoints.V1.Authorizations;

internal sealed class AuthorizeEndpoint : Endpoint<AuthorizeRequest, Guid>
{
    private readonly IAuthorizeService _authorizeService;


    public AuthorizeEndpoint(IAuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }

    public override void Configure()
    {
        Post("authenticate");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(AuthorizeRequest req, CancellationToken ct)
    {
        var userId = await _authorizeService.AuthorizeAsync(req.Authorization,
            req.Action,
            req.ServiceSecret,
            HttpContext.Connection.LocalIpAddress,
            ct);

        await SendOkAsync(userId, ct);
    }
}

internal sealed class AuthorizeEndpointSummary : Summary<AuthorizeEndpoint>
{
    public AuthorizeEndpointSummary()
    {
        Summary = "Authorize";
        Description = "Authorize by Token";
        Response<Certificate>(200, "Authorized successfully");
    }
}

public sealed record AuthorizeRequest
{
    [FromHeader] public string Authorization { get; init; } = default!;

    [FromHeader] public string Action { get; init; } = default!;

    [FromHeader] public string ServiceSecret { get; init; } = default!;
}

internal sealed class AuthorizeRequestValidator : AbstractValidator<AuthorizeRequest>
{
    public AuthorizeRequestValidator()
    {
        RuleFor(request => request.Authorization)
            .NotEmpty().WithMessage("Enter valid Authorization")
            .NotNull().WithMessage("Enter valid Authorization");

        RuleFor(request => request.Action)
            .NotEmpty().WithMessage("Enter valid Action")
            .NotNull().WithMessage("Enter valid Action");

        RuleFor(request => request.ServiceSecret)
            .NotEmpty().WithMessage("Enter valid ServiceSecret")
            .NotNull().WithMessage("Enter valid ServiceSecret");
    }
}