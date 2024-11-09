using Core.Auth.Authorizations;
using FastEndpoints;
using FluentValidation;

namespace Application.Auth.Authorizations.Role;

file sealed class Endpoint : Endpoint<Request, AuthorizeResponse>
{
    private readonly IRoleAuthorizationHandler _authorizationHandler;
    private const string UnAuthorizedMessage = "Unauthorized";
    private const string ForbiddenMessage = "Forbidden";
    private const string SessionExpiredMessage = "SessionExpired";

    public Endpoint(IRoleAuthorizationHandler authorizationHandler)
    {
        _authorizationHandler = authorizationHandler;
    }


    public override void Configure()
    {
        Post("authorize/role");
        ResponseCache(10, varyByHeader: "Authorization");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var authorizeResponse = await _authorizationHandler.HandleAsync(
            req.Authorization,
            req.ServiceSecret,
            req.Roles,
            HttpContext.Request.IpAddress(),
            HttpContext.Request.UserAgent(),
            ct);

        var authorizationResult = authorizeResponse.Code switch
        {
            200 => SendOkAsync(authorizeResponse, ct),
            401 => SendStringAsync(UnAuthorizedMessage, authorizeResponse.Code, cancellation: ct),
            403 => SendStringAsync(ForbiddenMessage, authorizeResponse.Code, cancellation: ct),
            440 => SendStringAsync(SessionExpiredMessage, authorizeResponse.Code, cancellation: ct),
            _ => SendStringAsync(UnAuthorizedMessage, 401, cancellation: ct)
        };

        await authorizationResult;
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permission Authorize";
        Description = "Authorize a role by Token";
        Response<AuthorizeResponse>(200, "Authorized successfully");
        Response(401, "UnAuthorized");
    }
}

file sealed record Request
{
    public string Authorization { get; init; } = default!;

    public string[] Roles { get; init; } = default!;

    [FromHeader("service_secret")] public string ServiceSecret { get; init; } = default!;
}

file sealed class RequestValidator : Validator<Request>
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