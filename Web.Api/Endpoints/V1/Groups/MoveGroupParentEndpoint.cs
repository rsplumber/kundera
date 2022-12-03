using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class MoveGroupParentEndpoint : Endpoint<MoveGroupParentCommand>
{
    private readonly IMediator _mediator;

    public MoveGroupParentEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{id:guid}/parent/move");
        Permissions("groups_move_parent");
        Version(1);
    }

    public override async Task HandleAsync(MoveGroupParentCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class MoveGroupParentEndpointSummary : Summary<MoveGroupParentEndpoint>
{
    public MoveGroupParentEndpointSummary()
    {
        Summary = "Move group parent in the system";
        Description = "Move group parent in the system";
        Response(200, "Parent was successfully moved");
    }
}