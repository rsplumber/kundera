using Core.Services;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Endpoints.V1.Auth.Authorizations.Permission;

internal sealed class Endpoint : Endpoint<Request, Guid>
{
    private readonly IAuthorizeService _authorizeService;

    public Endpoint(IAuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }

    public override void Configure()
    {
        Post("authorize/permission");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var userId = await _authorizeService.AuthorizePermissionAsync(req.Authorization,
            req.Actions,
            req.ServiceSecret,
            HttpContext.Connection.RemoteIpAddress,
            ct);

        await SendOkAsync(userId, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permission Authorize";
        Description = "Authorize a permission by Token";
        Response<Guid>(200, "Authorized successfully");
        Response<ForbidResult>(401, "UnAuthorized");
    }
}

internal sealed record Request
{
    public string Authorization { get; init; } = default!;

    public string[] Actions { get; init; } = default!;

    public string ServiceSecret { get; init; } = default!;
}

internal sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.Authorization)
            .NotEmpty().WithMessage("Enter valid Authorization")
            .NotNull().WithMessage("Enter valid Authorization");

        RuleFor(request => request.Actions)
            .NotEmpty().WithMessage("Enter valid Action")
            .NotNull().WithMessage("Enter valid Action");

        RuleFor(request => request.ServiceSecret)
            .NotEmpty().WithMessage("Enter valid ServiceSecret")
            .NotNull().WithMessage("Enter valid ServiceSecret");
    }
}