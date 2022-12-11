﻿using Application.Groups;
using Application.Scopes;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Scopes;

internal sealed class ScopesEndpoint : Endpoint<ScopesQuery, List<ScopesResponse>>
{
    private readonly IMediator _mediator;

    public ScopesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("scopes");
        Permissions("scopes_list");
        Version(1);
    }

    public override async Task HandleAsync(ScopesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class ScopesEndpointSummary : Summary<ScopesEndpoint>
{
    public ScopesEndpointSummary()
    {
        Summary = "Scopes list";
        Description = "Scopes list";
        Response<GroupResponse>(200, "Scopes was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}