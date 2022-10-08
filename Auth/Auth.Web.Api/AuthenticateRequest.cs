using FluentValidation;
using Tes.Web.Validators;

namespace Authentication.Web.Api;

public record AuthenticateRequest(string Username, string Password, string? Type = null, string Scope = "global") : IWebRequest;

public class AuthenticateRequestValidator : RequestValidator<AuthenticateRequest>
{
    public AuthenticateRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter valid Username")
            .NotNull().WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Enter valid Password")
            .NotNull().WithMessage("Enter valid Password");
    }
}