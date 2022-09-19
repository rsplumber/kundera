using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain.UserGroups;

namespace Users.Web.Api.Users;

public record CreateUserByEmailRequest(string Email, Guid UserGroup) : IWebRequest
{

    public CreateUserByEmailCommand ToCommand() => new(Email, UserGroupId.From(UserGroup));
}

public class CreateUserByEmailRequestValidator : RequestValidator<CreateUserByEmailRequest>
{
    public CreateUserByEmailRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Enter a valid Email")
            .NotNull().WithMessage("Enter a valid Email");
        
        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}