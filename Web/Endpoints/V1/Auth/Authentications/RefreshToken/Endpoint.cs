using Application.Auth.Authentications;
using Core.Domains.Auth.Authorizations;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Auth.Authentications.RefreshToken;

internal sealed class Endpoint : Endpoint<Request, Certificate>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("authenticate/refresh");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = new RefreshCertificateCommand
        {
            Token = req.Token,
            RefreshToken = req.RefreshToken,
            UserAgent = HttpContext.Request.UserAgent(),
            IpAddress = HttpContext.Request.IpAddress()
        };
        var response = await _mediator.Send(command, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Refresh token";
        Description = "Refresh expired token";
        Response<Certificate>(200, "Expired token refreshed");
    }
}

internal sealed record Request
{
    public string Token { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}

internal sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Enter valid Token")
            .NotNull().WithMessage("Enter valid Token");

        RuleFor(request => request.RefreshToken)
            .NotEmpty().WithMessage("Enter valid RefreshToken")
            .NotNull().WithMessage("Enter valid RefreshToken");
    }
}