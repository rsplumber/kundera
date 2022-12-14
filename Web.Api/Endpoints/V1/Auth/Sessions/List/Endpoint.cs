using Core.Domains.Users.Types;
using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Endpoints.V1.Auth.Sessions.List;

internal sealed class Endpoint : Endpoint<Request>
{
    private readonly ISessionManagement _sessionManagement;

    public Endpoint(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    public override void Configure()
    {
        Delete("users/{id:guid}/sessions");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var sessions = await _sessionManagement.GetAllAsync(UserId.From(req.Id), ct);
        var response = sessions.Select(session => new
        {
            session.Id, Scope = session.Scope, session.ExpiresAt, UserId = session.User, session.LastIpAddress, session.LastUsageDate
        });

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "User sessions";
        Description = "List of user active sessions";
        Response(200, "Sessions received successfully");
    }
}

internal sealed record Request
{
    public Guid Id { get; init; } = default!;
}

internal sealed class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Enter valid Id")
            .NotNull().WithMessage("Enter valid Id");
    }
}