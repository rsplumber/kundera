using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class DeleteGroupEndpoint : Endpoint<DeleteGroupCommand>
{
    private readonly IMediator _mediator;

    public DeleteGroupEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("groups/{id:guid}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(DeleteGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class DeleteGroupEndpointSummary : Summary<DeleteGroupEndpoint>
{
    public DeleteGroupEndpointSummary()
    {
        Summary = "Delete a  group in the system";
        Description = "Delete a group in the system";
        Response(204, "Group was successfully deleted");
    }
}