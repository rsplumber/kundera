using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Auth;

public record AuthorizeRequest(string Action) : IWebRequest;

public class AuthorizeRequestValidator : RequestValidator<AuthorizeRequest>
{
    public AuthorizeRequestValidator()
    {
        RuleFor(request => request.Action)
            .NotEmpty()
            .WithMessage("Enter valid Action")
            .NotNull()
            .WithMessage("Enter valid Action");
    }
}