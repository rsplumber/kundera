using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class SetGroupParentEndpoint : Endpoint<SetGroupParentCommand>
{
    private readonly IMediator _mediator;

    public SetGroupParentEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{id:guid}/parent");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(SetGroupParentCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class SetGroupParentEndpointSummary : Summary<SetGroupParentEndpoint>
{
    public SetGroupParentEndpointSummary()
    {
        Summary = "Set a parent to a group in the system";
        Description = "Set a parent to a group in the system";
        Response(200, "Parent was successfully set to the group");
    }
}