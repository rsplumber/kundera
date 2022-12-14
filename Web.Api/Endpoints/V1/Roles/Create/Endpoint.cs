using Application.Roles;
using FastEndpoints;
using Mediator;
using Web.Api.Endpoints.V1.Roles.Details;

namespace Web.Api.Endpoints.V1.Roles.Create;

internal sealed class Endpoint : Endpoint<CreateRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("roles");
        Permissions("roles_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateRoleCommand req, CancellationToken ct)
    {
        var role = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<Details.Endpoint>(new {role.Id}, new RoleResponse
            {
                Id = role.Id.Value,
                Name = role.Name
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new role in the system";
        Description = "Create a new role in the system";
        Response(201, "Role was successfully created");
    }
}