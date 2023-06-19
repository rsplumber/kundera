using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Users.Roles.Revoke;

internal sealed class Endpoint : Endpoint<RevokeUserRoleCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("users/{userId:guid}/roles");
        Permissions("users_revoke_role");
        Version(1);
    }

    public override async Task HandleAsync(RevokeUserRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}

internal sealed class EndpointSSummary : Summary<Endpoint>
{
    public EndpointSSummary()
    {
        Summary = "Revoke a role from a user in the system";
        Description = "Revoke a role from a user a in the system";
        Response(204, "Role was successfully revoked from the user");
    }
}

internal sealed class RequestValidator : Validator<RevokeUserRoleCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty().WithMessage("Enter a UserId")
            .NotNull().WithMessage("Enter a UserId");

        RuleFor(request => request.RolesIds)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}