using Application.Groups;
using Application.Permissions;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Permissions;

internal sealed class PermissionsEndpoint : Endpoint<PermissionsQuery, List<PermissionsResponse>>
{
    private readonly IMediator _mediator;

    public PermissionsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("permissions");
        Permissions("permissions_list");
        Version(1);
    }

    public override async Task HandleAsync(PermissionsQuery req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}

internal sealed class PermissionsEndpointSummary : Summary<PermissionsEndpoint>
{
    public PermissionsEndpointSummary()
    {
        Summary = "Permissions list";
        Description = "Permissions list";
        Response<GroupResponse>(200, "Permissions was successfully received");
        Response<ValidationFailureResponse>(400, "The request did not pass validation checks");
    }
}