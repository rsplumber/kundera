using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Groups.Role.Assign;

file sealed class Endpoint : Endpoint<AssignGroupRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups/{groupId:guid}/roles");
        Permissions("groups_assign_role");
        Version(1);
    }

    public override async Task HandleAsync(AssignGroupRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendOkAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Assign a role to a group in the system";
        Description = "Assign a role to a group in the system";
        Response(200, "Role was successfully assigned to the group");
    }
}

file sealed class RequestValidator : Validator<AssignGroupRoleCommand>
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