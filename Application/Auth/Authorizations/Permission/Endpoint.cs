using Core.Auth.Authorizations;
using FastEndpoints;
using FluentValidation;

namespace Application.Auth.Authorizations.Permission;

internal sealed class Endpoint : Endpoint<Request, AuthorizeResponse>
{
    private readonly IAuthorizeService _authorizeService;
    private const string UnAuthorizedMessage = "Unauthorized";
    private const string ForbiddenMessage = "Forbidden";
    private const string SessionExpiredMessage = "SessionExpired";

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
        var (authorize, unAuthorized) = await _authorizeService.AuthorizePermissionAsync(req.Authorization,
            req.Actions,
            req.ServiceSecret,
            HttpContext.Request.UserAgent(),
            HttpContext.Request.IpAddress().ToString(),
            ct);

        switch (authorize)
        {
            case null when unAuthorized is not null:
                await SendStringAsync(GetUnAuthorizedMessage(), unAuthorized.Code, cancellation: ct);
                return;
            case null when unAuthorized is null:
                await SendStringAsync(UnAuthorizedMessage, 403, cancellation: ct);
                return;
        }

        await SendOkAsync(authorize!, ct);

        string GetUnAuthorizedMessage() => unAuthorized.Code switch
        {
            401 => ForbiddenMessage,
            403 => UnAuthorizedMessage,
            440 => SessionExpiredMessage,
            _ => UnAuthorizedMessage
        };
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permission Authorize";
        Description = "Authorize a permission by Token";
        Response<AuthorizeResponse>(200, "Authorized successfully");
        Response(401, "UnAuthorized");
    }
}

internal sealed record Request
{
    public string Authorization { get; init; } = default!;

    public string[] Actions { get; init; } = default!;

    [FromHeader("service_secret")] public string ServiceSecret { get; init; } = default!;
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