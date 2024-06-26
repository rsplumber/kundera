﻿using Data.Abstractions.Groups;
using Data.Abstractions.Scopes;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Details;

file sealed class Endpoint : Endpoint<ScopeQuery, ScopeResponse>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("scopes/{scopeId:guid}");
        Permissions("scopes_get");
        Version(1);
    }

    public override async Task HandleAsync(ScopeQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Scope details";
        Description = "Scope details";
        Response<GroupResponse>(200, "Scope was successfully received");
    }
}

file sealed class RequestValidator : Validator<ScopeQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a ScopeId")
            .NotNull().WithMessage("Enter a ScopeId");
    }
}