using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Web.Api.Users;

public record AssignUserRoleRequest(Guid User, List<string> RoleIds) : IWebRequest
{
    public AssignUserRoleCommand ToCommand() => new(UserId.From(User), RoleIds.Select(RoleId.From).ToArray());
}

public class AssignUserRoleRequestValidator : RequestValidator<AssignUserRoleRequest>
{
    public AssignUserRoleRequestValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter a valid User")
            .NotNull().WithMessage("Enter a valid User");
    }
}

public record RevokeUserRoleRequest(Guid User, List<string> RoleIds) : IWebRequest
{
    public RevokeUserRoleCommand ToCommand() => new(UserId.From(User), RoleIds.Select(RoleId.From).ToArray());
}

public class RevokeUserRoleRequestValidator : RequestValidator<RevokeUserRoleRequest>
{
    public RevokeUserRoleRequestValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter a valid User")
            .NotNull().WithMessage("Enter a valid User");
    }
}