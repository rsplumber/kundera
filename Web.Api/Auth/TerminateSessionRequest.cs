using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Auth;

public record TerminateSessionRequest(string Token) : IWebRequest;

public class TerminateSessionRequestValidator : RequestValidator<TerminateSessionRequest>
{
    public TerminateSessionRequestValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty()
            .WithMessage("Enter valid Token")
            .NotNull()
            .WithMessage("Enter valid Token");
    }
}