using FluentValidation;
using Tes.Web.Validators;
using Users.Application.UserGroups;

namespace Users.Web.Api.UserGroups;

public record CreateUserGroupRequest(string Name) : IWebRequest
{
    public CreateUserGroupCommand ToCommand() => new(Name);
}

public class CreateUserGroupRequestValidator : RequestValidator<CreateUserGroupRequest>
{
    public CreateUserGroupRequestValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(6).WithMessage("Email minimum length 3")
            .NotEmpty().WithMessage("Enter a valid Email")
            .NotNull().WithMessage("Enter a valid Email");
    }
}