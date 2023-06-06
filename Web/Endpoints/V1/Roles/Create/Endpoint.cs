using Application.Roles;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Roles.Create;

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

        await SendCreatedAtAsync<Details.Endpoint>(new { role.Id }, new RoleResponse
            {
                Id = role.Id,
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

internal sealed class RequestValidator : Validator<CreateRoleCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter a Name")
            .NotNull().WithMessage("Enter a Name");
    }
}