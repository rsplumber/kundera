﻿using Core.Services;
using FastEndpoints;
using FluentValidation;

namespace Auth.Application.Endpoints.V1.Sessions;

internal sealed class UserSessionsEndpoint : Endpoint<UserSessionsRequest>
{
    private readonly ISessionManagement _sessionManagement;

    public UserSessionsEndpoint(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    public override void Configure()
    {
        Delete("users/{id:guid}/sessions");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(UserSessionsRequest req, CancellationToken ct)
    {
        var sessions = await _sessionManagement.GetAllAsync(req.Id, ct);
        var response = sessions.Select(session => new
        {
            session.Id, Scope = session.ScopeId, session.ExpiresAt, session.UserId, session.LastIpAddress, session.LastUsageDate
        });

        await SendOkAsync(response, ct);
    }
}

internal sealed class UserSessionsEndpointSummary : Summary<UserSessionsEndpoint>
{
    public UserSessionsEndpointSummary()
    {
        Summary = "User sessions";
        Description = "List of user active sessions";
        Response(200, "Sessions received successfully");
    }
}

public sealed record UserSessionsRequest
{
    public Guid Id { get; init; } = default!;
}

public class UserSessionsRequestValidator : AbstractValidator<UserSessionsRequest>
{
    public UserSessionsRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Enter valid Id")
            .NotNull().WithMessage("Enter valid Id");
    }
}