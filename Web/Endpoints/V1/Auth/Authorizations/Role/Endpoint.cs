using Core.Domains.Auth.Authorizations;
using FastEndpoints;
using FluentValidation;

namespace Web.Endpoints.V1.Auth.Authorizations.Role;

internal sealed class Endpoint : Endpoint<Request, Guid>
{
    private readonly IAuthorizeService _authorizeService;

    public Endpoint(IAuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }

    public override void Configure()
    {
        Post("authorize/role");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var userId = await _authorizeService.AuthorizeRoleAsync(req.Authorization,
            req.Roles,
            req.ServiceSecret,
            HttpContext.Request.UserAgent(),
            HttpContext.Request.IpAddress().ToString(),
            ct);

        await SendOkAsync(userId, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permission Authorize";
        Description = "Authorize a role by Token";
        Response<Guid>(200, "Authorized successfully");
        Response(401, "UnAuthorized");
    }
}

internal sealed record Request
{
    public string Authorization { get; init; } = default!;

    public string[] Roles { get; init; } = default!;

    [FromHeader("service_secret")] public string ServiceSecret { get; init; } = default!;
}

internal sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.Authorization)
            .NotEmpty().WithMessage("Enter valid Authorization")
            .NotNull().WithMessage("Enter valid Authorization");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter valid Role")
            .NotNull().WithMessage("Enter valid Role");

        RuleFor(request => request.ServiceSecret)
            .NotEmpty().WithMessage("Enter valid ServiceSecret")
            .NotNull().WithMessage("Enter valid ServiceSecret");
    }
}