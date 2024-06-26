﻿using Data.Abstractions;
using Data.Abstractions.Groups;
using Data.Abstractions.Roles;
using FastEndpoints;
using Mediator;

namespace Application.Roles.List;

file sealed class Endpoint : Endpoint<RolesQuery, PageableResponse<RolesResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("roles");
        Permissions("roles_list");
        Version(1);
    }

    public override async Task HandleAsync(RolesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Roles list";
        Description = "Roles list";
        Response<PageableResponse<GroupResponse>>(200, "Roles was successfully received");
    }
}