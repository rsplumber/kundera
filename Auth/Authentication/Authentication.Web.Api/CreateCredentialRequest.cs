using FluentValidation;
using Tes.Web.Validators;

namespace Authentication.Web.Api;

public record CreateCredentialRequest(string Username, string Password, string? Type = null) : IWebRequest;

public class CreateCredentialRequestValidator : RequestValidator<CreateCredentialRequest>
{
    public CreateCredentialRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");
    }
}