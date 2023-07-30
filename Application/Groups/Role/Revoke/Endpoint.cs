using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Groups.Role.Revoke;

file sealed class Endpoint : Endpoint<RevokeGroupRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("groups/{groupId:guid}/roles");
        Permissions("groups_revoke_role");
        Version(1);
    }

    public override async Task HandleAsync(RevokeGroupRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Revoke a role from a group in the system";
        Description = "Revoke a role from a group a in the system";
        Response(204, "Role was successfully revoked from the user");
    }
}

file sealed class RequestValidator : Validator<RevokeGroupRoleCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");

        RuleFor(request => request.RolesIds)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}