using Core.Services;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Endpoints.V1.Auth.Authorizations;

internal sealed class AuthorizeRoleEndpoint : Endpoint<AuthorizeRoleRequest, Guid>
{
    private readonly IAuthorizeService _authorizeService;

    public AuthorizeRoleEndpoint(IAuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }

    public override void Configure()
    {
        Post("authorize/role");
        AllowAnonymous();
        Version(1);
        ResponseCache(5);
    }

    public override async Task HandleAsync(AuthorizeRoleRequest req, CancellationToken ct)
    {
        var userId = await _authorizeService.AuthorizeRoleAsync(req.Authorization,
            req.Roles,
            req.ServiceSecret,
            HttpContext.Connection.RemoteIpAddress,
            ct);

        await SendOkAsync(userId, ct);
    }
}

internal sealed class AuthorizeRoleEndpointSummary : Summary<AuthorizeRoleEndpoint>
{
    public AuthorizeRoleEndpointSummary()
    {
        Summary = "Permission Authorize";
        Description = "Authorize a role by Token";
        Response<Guid>(200, "Authorized successfully");
        Response<ForbidResult>(401, "UnAuthorized");
    }
}

public sealed record AuthorizeRoleRequest
{
    public string Authorization { get; init; } = default!;

    public string[] Roles { get; init; } = default!;

    public string ServiceSecret { get; init; } = default!;
}

internal sealed class AuthorizeRoleRequestValidator : Validator<AuthorizeRoleRequest>
{
    public AuthorizeRoleRequestValidator()
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