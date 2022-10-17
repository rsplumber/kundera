using FluentValidation;
using Kite.Web.Requests;

namespace Web.Apix.Auth;

public record CredentialChangePasswordRequest(string Password, string NewPassword) : IWebRequest;

public class CredentialChangePasswordRequestValidator : RequestValidator<CredentialChangePasswordRequest>
{
    public CredentialChangePasswordRequestValidator()
    {
        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Enter valid Password")
            .NotNull()
            .WithMessage("Enter valid Password");

        RuleFor(request => request.NewPassword)
            .NotEmpty()
            .WithMessage("Enter valid NewPassword")
            .NotNull()
            .WithMessage("Enter valid NewPassword");
    }
}