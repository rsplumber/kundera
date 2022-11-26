using Application.Groups;
using Application.Roles;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles;

internal sealed class RolesEndpoint : Endpoint<RolesQuery, List<RolesResponse>>
{
    private readonly IMediator _mediator;

    public RolesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("roles");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RolesQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class RolesEndpointSummary : Summary<RolesEndpoint>
{
    public RolesEndpointSummary()
    {
        Summary = "Roles list";
        Description = "Roles list";
        Response<GroupResponse>(200, "Roles was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}