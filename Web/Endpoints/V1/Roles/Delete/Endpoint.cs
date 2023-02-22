using Commands.Roles;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Roles.Delete;

internal sealed class Endpoint : Endpoint<DeleteRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("roles/{roleId:guid}");
        Permissions("roles_delete");
        Version(1);
    }

    public override async Task HandleAsync(DeleteRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Delete a role in the system";
        Description = "Delete a role in the system";
        Response(204, "Role was successfully deleted");
    }
}

internal sealed class RequestValidator : Validator<DeleteRoleCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter a RoleId")
            .NotNull().WithMessage("Enter a RoleId");
    }
}