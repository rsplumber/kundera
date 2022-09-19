using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain.UserGroups;

namespace Users.Web.Api.Users;

public record CreateUserByUsernameRequest(string Username, Guid UserGroup) : IWebRequest
{
    public CreateUserByUsernameCommand ToCommand() => new(Username, UserGroupId.From(UserGroup));
}

public class CreateUserByUsernameRequestValidator : RequestValidator<CreateUserByUsernameRequest>
{
    public CreateUserByUsernameRequestValidator()
    {
        RuleFor(request => request.Username)
            .MinimumLength(2).WithMessage("Username minimum length is 2")
            .MaximumLength(30).WithMessage("Username Maximum length is 30")
            .NotEmpty().WithMessage("Enter a valid Username")
            .NotNull().WithMessage("Enter a valid Username");

        RuleFor(request => request.UserGroup)
            .NotEmpty().WithMessage("Enter a valid UserGroup")
            .NotNull().WithMessage("Enter a valid UserGroup");
    }
}

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