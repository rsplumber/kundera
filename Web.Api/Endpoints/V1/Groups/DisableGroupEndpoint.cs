using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class DisableGroupEndpoint : Endpoint<DisableGroupCommand>
{
    private readonly IMediator _mediator;

    public DisableGroupEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{groupId:guid}/disable");
        Permissions("groups_disable");
        Version(1);
    }

    public override async Task HandleAsync(DisableGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class DisableGroupEndpointSummary : Summary<DisableGroupEndpoint>
{
    public DisableGroupEndpointSummary()
    {
        Summary = "Disable a group in the system";
        Description = "Disable a group in the system";
        Response(200, "Group was successfully Disabled");
    }
}