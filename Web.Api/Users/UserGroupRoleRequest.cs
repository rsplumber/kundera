using Application.Users;
using Domain.Roles;
using Domain.Users;
using FluentValidation;
using Tes.Web.Validators;

namespace Web.Api.Users;

public record AssignUserRoleRequest(List<string> RoleIds) : IWebRequest
{
    public AssignUserRoleCommand ToCommand(Guid userId) => new(UserId.From(userId), RoleIds.Select(RoleId.From).ToArray());
}

public class AssignUserRoleRequestValidator : RequestValidator<AssignUserRoleRequest>
{
    public AssignUserRoleRequestValidator()
    {
        RuleFor(request => request.RoleIds)
            .NotEmpty().WithMessage("Enter a valid User")
            .NotNull().WithMessage("Enter a valid User");
    }
}

public record RevokeUserRoleRequest(List<string> RoleIds) : IWebRequest
{
    public RevokeUserRoleCommand ToCommand(Guid userId) => new(UserId.From(userId), RoleIds.Select(RoleId.From).ToArray());
}

public class RevokeUserRoleRequestValidator : RequestValidator<RevokeUserRoleRequest>
{
    public RevokeUserRoleRequestValidator()
    {
        RuleFor(request => request.RoleIds)
            .NotEmpty().WithMessage("Enter a valid User")
            .NotNull().WithMessage("Enter a valid User");
    }
}