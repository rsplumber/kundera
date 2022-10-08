using FluentValidation;
using Tes.Web.Validators;

namespace Authentication.Web.Api;

public record CredentialChangePasswordRequest(string Password, string NewPassword) : IWebRequest;

public class CredentialChangePasswordRequestValidator : RequestValidator<CredentialChangePasswordRequest>
{
    public CredentialChangePasswordRequestValidator()
    {
        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");

        RuleFor(request => request.NewPassword)
            .NotEmpty().WithMessage("Enter valid NewPassword")
            .NotNull().WithMessage("Enter valid NewPassword");
    }
}