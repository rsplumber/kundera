using FluentValidation;
using Tes.Web.Validators;

namespace Authentication.Web.Api;

public record UpdateCredentialRequest(string Password) : IWebRequest;

public class UpdateCredentialRequestValidator : RequestValidator<UpdateCredentialRequest>
{
    public UpdateCredentialRequestValidator()
    {
        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");
    }
}