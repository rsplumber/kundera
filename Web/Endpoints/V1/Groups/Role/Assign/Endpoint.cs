using Commands.Groups;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Web.Endpoints.V1.Groups.Role.Assign;

internal sealed class Endpoint : Endpoint<AssignGroupRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{groupId:guid}/roles");
        Permissions("kundera_groups_assign_role");
        Version(1);
    }

    public override async Task HandleAsync(AssignGroupRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Assign a role to a group in the system";
        Description = "Assign a role to a group in the system";
        Response(200, "Role was successfully assigned to the group");
    }
}

internal sealed class RequestValidator : Validator<AssignGroupRoleCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");

        RuleFor(request => request.Roles)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}