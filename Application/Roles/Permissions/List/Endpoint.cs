﻿using Data.Abstractions.Groups;
using Data.Abstractions.Roles;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Roles.Permissions.List;

file sealed class Endpoint : Endpoint<RolePermissionsQuery, List<RolePermissionsResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("roles/{roleId:guid}/permissions");
        Permissions("roles_permissions_list");
        Version(1);
    }

    public override async Task HandleAsync(RolePermissionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Role permissions list";
        Description = "Roles permissions list";
        Response<GroupResponse>(200, "Role permissions was successfully received");
    }
}

file sealed class RequestValidator : Validator<RolePermissionsQuery>
{
    public RequestValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter RoleId")
            .NotNull().WithMessage("Enter RoleId");
    }
}