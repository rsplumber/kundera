using Application.Groups;
using Application.Roles;
using FastEndpoints;
using Mediator;

namespace Web.Endpoints.V1.Roles.List;

internal sealed class Endpoint : Endpoint<RolesQuery, List<RolesResponse>>
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

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Roles list";
        Description = "Roles list";
        Response<GroupResponse>(200, "Roles was successfully received");
    }
}