﻿using Data.Abstractions.Groups;
using Data.Abstractions.Scopes;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Scopes.Roles.List;

file sealed class Endpoint : Endpoint<ScopeRolesQuery, List<ScopeRolesResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("scopes/{scopeId:guid}/roles");
        Permissions("scopes_roles_list");
        Version(1);
    }

    public override async Task HandleAsync(ScopeRolesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Scope roles list";
        Description = "Scope roles list";
        Response<GroupResponse>(200, "Scope roles was successfully received");
    }
}

file sealed class RequestValidator : Validator<ScopeRolesQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");
    }
}