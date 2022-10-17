using FluentValidation;
using Kite.Web.Requests;

namespace Web.Apix.Auth;

public record AuthenticateRequest(string Username, string Password, string? Type = null, string Scope = "global") : IWebRequest;

public class AuthenticateRequestValidator : RequestValidator<AuthenticateRequest>
{
    public AuthenticateRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty()
            .WithMessage("Enter valid Username")
            .NotNull()
            .WithMessage("Enter valid Username");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Enter valid Password")
            .NotNull()
            .WithMessage("Enter valid Password");
    }
}

public record RefreshRequest(string RefreshToken) : IWebRequest;

public class RefreshRequestValidator : RequestValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .WithMessage("Enter valid RefreshToken")
            .NotNull()
            .WithMessage("Enter valid RefreshToken");
    }
}