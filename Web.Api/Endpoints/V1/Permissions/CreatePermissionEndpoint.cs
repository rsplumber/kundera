using FastEndpoints;
using Managements.Application.Permissions;
using Mediator;

namespace Web.Api.Endpoints.V1.Permissions;

internal sealed class CreatePermissionEndpoint : Endpoint<CreatePermissionCommand>
{
    private readonly IMediator _mediator;

    public CreatePermissionEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("permissions");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreatePermissionCommand req, CancellationToken ct)
    {
        var service = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<PermissionEndpoint>(new {service.Id}, new PermissionResponse
            {
                Id = service.Id.Value,
                Name = service.Name,
                Meta = (Dictionary<string, string>) service.Meta
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class CreatePermissionEndpointSummary : Summary<CreatePermissionEndpoint>
{
    public CreatePermissionEndpointSummary()
    {
        Summary = "Create a new Permission in the system";
        Description = "Create a new Permission in the system";
        Response(201, "Permission was successfully created");
    }
}