using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain.UserGroups;

namespace Users.Web.Api.Users;

public record CreateUserByPhoneNumberRequest(string PhoneNumber, Guid UserGroup) : IWebRequest
{

    public CreateUserByPhoneNumberCommand ToCommand() => new(PhoneNumber, UserGroupId.From(UserGroup));
}

public class CreateUserByPhoneNumberRequestValidator : RequestValidator<CreateUserByUsernameRequest>
{
    public CreateUserByPhoneNumberRequestValidator()
    {
        RuleFor(request => request.Username)
            .Length(11).WithMessage("PhoneNumber length is 11")
            .NotEmpty().WithMessage("Enter a valid PhoneNumber")
            .NotNull().WithMessage("Enter a valid PhoneNumber");
        
        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}