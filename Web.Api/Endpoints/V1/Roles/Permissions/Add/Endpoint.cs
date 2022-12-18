using Application.Roles;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Api.Endpoints.V1.Roles.Permissions.Add;

internal sealed class Endpoint : Endpoint<AddRolePermissionCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("roles/{roleId:guid}/permissions");
        Permissions("roles_add_permission");
        Version(1);
    }

    public override async Task HandleAsync(AddRolePermissionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Add role permission in the system";
        Description = "Add role permission in the system";
        Response(200, "Role permission was successfully added");
    }
}

internal sealed class RequestValidator : Validator<AddRolePermissionCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter a RoleId")
            .NotNull().WithMessage("Enter a RoleId");

        RuleFor(request => request.Permissions)
            .NotEmpty().WithMessage("Enter a at least one permission")
            .NotNull().WithMessage("Enter a at least one permission");
    }
}