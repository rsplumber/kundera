using Application.Roles;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles;

internal sealed class CreateRoleEndpoint : Endpoint<CreateRoleCommand>
{
    private readonly IMediator _mediator;

    public CreateRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("roles");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(CreateRoleCommand req, CancellationToken ct)
    {
        var role = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<RoleEndpoint>(new {role.Id}, new RoleResponse
            {
                Id = role.Id.Value,
                Name = role.Name
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class CreateRoleEndpointSummary : Summary<CreateRoleEndpoint>
{
    public CreateRoleEndpointSummary()
    {
        Summary = "Create a new role in the system";
        Description = "Create a new role in the system";
        Response(201, "Role was successfully created");
    }
}