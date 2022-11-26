using FastEndpoints;
using Managements.Application.Groups;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class EnableGroupEndpoint : Endpoint<EnableGroupCommand>
{
    private readonly IMediator _mediator;

    public EnableGroupEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{id:guid}/enable");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(EnableGroupCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EnableGroupEndpointSummary : Summary<EnableGroupEndpoint>
{
    public EnableGroupEndpointSummary()
    {
        Summary = "Enable a group in the system";
        Description = "Enable a group in the system";
        Response(200, "Group was successfully Enabled");
    }
}