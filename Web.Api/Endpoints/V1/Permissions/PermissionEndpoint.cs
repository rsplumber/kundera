﻿using FastEndpoints;
using Managements.Application.Groups;
using Managements.Application.Permissions;
using Mediator;

namespace Web.Api.Endpoints.V1.Permissions;

internal sealed class PermissionEndpoint : Endpoint<PermissionQuery, PermissionResponse>
{
    private readonly IMediator _mediator;

    public PermissionEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("permissions/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(PermissionQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class PermissionEndpointSummary : Summary<PermissionEndpoint>
{
    public PermissionEndpointSummary()
    {
        Summary = "Permission details";
        Description = "Permission details";
        Response<GroupResponse>(200, "Permission was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}