using FluentValidation;
using Tes.Web.Validators;
using Users.Application.Users;
using Users.Domain.UserGroups;

namespace Users.Web.Api.Users;

//Todo Khob Hame CreateUserato biar inja dg, man yedonasho ovordam, vase role o group am hamin karo bkon vaqti khasti request bezani, nemonsash: ScopeRoleRequest
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