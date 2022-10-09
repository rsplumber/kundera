using FluentValidation;
using Tes.Web.Validators;

namespace Authentication.Web.Api;

public record AuthorizeRequest(string Token, string Action, string Scope = "global") : IWebRequest;

public class AuthorizeRequestValidator : RequestValidator<AuthorizeRequest>
{
    public AuthorizeRequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Enter valid Token")
            .NotNull().WithMessage("Enter valid Token");

        RuleFor(request => request.Action)
            .NotEmpty().WithMessage("Enter valid Action")
            .NotNull().WithMessage("Enter valid Action");
    }
}