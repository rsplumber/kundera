using Application.Groups;
using Application.Roles;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles;

internal sealed class RoleEndpoint : Endpoint<RoleQuery, RoleResponse>
{
    private readonly IMediator _mediator;

    public RoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("roles/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RoleQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class RoleEndpointSummary : Summary<RoleEndpoint>
{
    public RoleEndpointSummary()
    {
        Summary = "Role details";
        Description = "Role details";
        Response<GroupResponse>(200, "Role was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}