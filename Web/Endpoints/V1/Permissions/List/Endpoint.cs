﻿using FastEndpoints;
using Mediator;
using Queries.Groups;
using Queries.Permissions;

namespace Web.Endpoints.V1.Permissions.List;

internal sealed class Endpoint : Endpoint<PermissionsQuery, List<PermissionsResponse>>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("permissions");
        Permissions("kundera_permissions_list");
        Version(1);
    }

    public override async Task HandleAsync(PermissionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Permissions list";
        Description = "Permissions list";
        Response<GroupResponse>(200, "Permissions was successfully received");
    }
}