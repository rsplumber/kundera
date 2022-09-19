using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain.UserGroups;

namespace Users.Web.Api.Users;

public record CreateUserByNationalCodeRequest(string NationalCode, Guid UserGroup) : IWebRequest
{

    public CreateUserByNationalCodeCommand ToCommand() => new(NationalCode, UserGroupId.From(UserGroup));
}

public class CreateUserByNationalCodeRequestValidator : RequestValidator<CreateUserByNationalCodeRequest>
{
    public CreateUserByNationalCodeRequestValidator()
    {
        RuleFor(request => request.NationalCode)
            .Length(10).WithMessage("Enter valid NationalCode")
            .NotEmpty().WithMessage("Enter a valid NationalCode")
            .NotNull().WithMessage("Enter a valid NationalCode");
        
        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}