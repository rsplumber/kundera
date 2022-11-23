using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Users;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Types;
using Managements.Domain.Users;
using Managements.Domain.Users.Types;

namespace Web.Api.Users;

public record AssignUserRoleRequest(List<Guid> RoleIds) : IWebRequest
{
    public AssignUserRoleCommand ToCommand(Guid userId) => new(UserId.From(userId),
        RoleIds.Select(RoleId.From)
            .ToArray());
}

public class AssignUserRoleRequestValidator : RequestValidator<AssignUserRoleRequest>
{
    public AssignUserRoleRequestValidator()
    {
        RuleFor(request => request.RoleIds)
            .NotEmpty()
            .WithMessage("Enter a valid User")
            .NotNull()
            .WithMessage("Enter a valid User");
    }
}

public record RevokeUserRoleRequest(List<Guid> RoleIds) : IWebRequest
{
    public RevokeUserRoleCommand ToCommand(Guid userId) => new(UserId.From(userId),
        RoleIds.Select(RoleId.From)
            .ToArray());
}

public class RevokeUserRoleRequestValidator : RequestValidator<RevokeUserRoleRequest>
{
    public RevokeUserRoleRequestValidator()
    {
        RuleFor(request => request.RoleIds)
            .NotEmpty()
            .WithMessage("Enter a valid User")
            .NotNull()
            .WithMessage("Enter a valid User");
    }
}