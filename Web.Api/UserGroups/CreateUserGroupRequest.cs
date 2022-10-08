using Application.UserGroups;
using FluentValidation;
using Tes.Web.Validators;

namespace Web.Api.UserGroups;

public record CreateUserGroupRequest(string Name, string RoleId) : IWebRequest
{
    public CreateUserGroupCommand ToCommand() => new(Name, Domain.Roles.RoleId.From(RoleId));
}

public class CreateUserGroupRequestValidator : RequestValidator<CreateUserGroupRequest>
{
    public CreateUserGroupRequestValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(6).WithMessage("UserGroup name minimum length 3")
            .NotEmpty().WithMessage("Enter a valid UserGroup name")
            .NotNull().WithMessage("Enter a valid UserGroup name");
    }
}