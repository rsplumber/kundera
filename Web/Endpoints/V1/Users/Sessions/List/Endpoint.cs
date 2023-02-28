﻿using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Users;

namespace Web.Endpoints.V1.Users.Sessions.List;

internal sealed class Endpoint : Endpoint<UserSessionsQuery, List<UserSessionResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("users/{userId:guid}/sessions");
        Permissions("kundera_users_sessions_list");
        Version(1);
    }

    public override async Task HandleAsync(UserSessionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);
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

internal sealed class RequestValidator : Validator<UserSessionsQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter valid UserId")
            .NotNull().WithMessage("Enter valid UserId");
    }
}