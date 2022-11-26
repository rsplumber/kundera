using FastEndpoints;
using Managements.Application.Groups;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class RemoveGroupParentEndpoint : Endpoint<RemoveGroupParentCommand>
{
    private readonly IMediator _mediator;

    public RemoveGroupParentEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("groups/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(RemoveGroupParentCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class RemoveGroupParentEndpointSummary : Summary<RemoveGroupParentEndpoint>
{
    public RemoveGroupParentEndpointSummary()
    {
        Summary = "Remove a parent from a group in the system";
        Description = "Set a parent from a group in the system";
        Response(204, "Parent was successfully removed from the group");
    }
}