using Application.Roles;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Roles.Permissions.Delete;

internal sealed class Endpoint : Endpoint<RemoveRolePermissionCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("roles/{roleId:guid}/permission");
        Permissions("roles_remove_permission");
        Version(1);
    }

    public override async Task HandleAsync(RemoveRolePermissionCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Remove role permission in the system";
        Description = "Remove role permission in the system";
        Response(204, "Role permission was successfully removed");
    }
}

internal sealed class RequestValidator : Validator<RemoveRolePermissionCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter a RoleId")
            .NotNull().WithMessage("Enter a RoleId");

        RuleFor(request => request.PermissionsIds)
            .NotEmpty().WithMessage("Enter a at least one permission")
            .NotNull().WithMessage("Enter a at least one permission");
    }
}