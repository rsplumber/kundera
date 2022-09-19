using FluentValidation;
using Tes.Web.Validators;
using Users.Application.UserGroups;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Web.Api.UserGroups;

public record AssignUserGroupRoleRequest(Guid UserGroup, List<string> RoleIds) : IWebRequest
{
    public AssignUserGroupRoleCommand ToCommand() => new(UserGroupId.From(UserGroup), RoleIds.Select(RoleId.From).ToArray());
}

public class AssignUserGroupRoleRequestValidator : RequestValidator<AssignUserGroupRoleRequest>
{
    public AssignUserGroupRoleRequestValidator()
    {
        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}

public record RevokeUserGroupRoleRequest(Guid UserGroup, List<string> RoleIds) : IWebRequest
{
    public RevokeUserGroupRoleCommand ToCommand() => new(UserGroupId.From(UserGroup), RoleIds.Select(RoleId.From).ToArray());
}

public class RevokeUserGroupRoleRequestValidator : RequestValidator<RevokeUserGroupRoleRequest>
{
    public RevokeUserGroupRoleRequestValidator()
    {
        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}