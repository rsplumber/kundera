using Application.Permissions;
using FastEndpoints;
using Mediator;
using Web.Api.Endpoints.V1.Permissions.Details;

namespace Web.Api.Endpoints.V1.Permissions.Create;

internal sealed class Endpoint : Endpoint<CreatePermissionCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("permissions");
        Permissions("permissions_create");
        Version(1);
    }

    public override async Task HandleAsync(CreatePermissionCommand req, CancellationToken ct)
    {
        var service = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<Details.Endpoint>(new {service.Id}, new PermissionResponse
            {
                Id = service.Id.Value,
                Name = service.Name,
                Meta = (Dictionary<string, string>) service.Meta
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new Permission in the system";
        Description = "Create a new Permission in the system";
        Response(201, "Permission was successfully created");
    }
}