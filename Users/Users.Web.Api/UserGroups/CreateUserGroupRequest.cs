using FluentValidation;
using Tes.Web.Validators;
using Users.Application.UserGroups;
using Users.Domain;

namespace Users.Web.Api.UserGroups;

public record CreateUserGroupRequest(string Name, List<string> RoleIds) : IWebRequest
{
    public CreateUserGroupCommand ToCommand() => new(Name, RoleIds.Select(RoleId.From).ToArray());
}

public class CreateUserGroupRequestValidator : RequestValidator<CreateUserGroupRequest>
{
    public CreateUserGroupRequestValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(3).WithMessage("UserGroup minimum length 3")
            .NotEmpty().WithMessage("Enter a valid Email")
            .NotNull().WithMessage("Enter a valid Email");

        RuleFor(request => request.RoleIds)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}