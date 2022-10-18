using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Auth;

public record CreateCredentialRequest(string Username, string Password, string? Type = null) : IWebRequest;

public class CreateCredentialRequestValidator : RequestValidator<CreateCredentialRequest>
{
    public CreateCredentialRequestValidator()
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

public record CreateOneTimeCredentialRequest(string Username, string Password, string? Type = null, int ExpirationTimeInSeconds = 0) : IWebRequest;

public class CreateOneTimeCredentialRequestValidator : RequestValidator<CreateOneTimeCredentialRequest>
{
    public CreateOneTimeCredentialRequestValidator()
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

public record CreateTimePeriodicCredentialRequest(string Username, string Password, int ExpirationTimeInSeconds, string? Type = null) : IWebRequest;

public class CreateTimePeriodicCredentialRequestValidator : RequestValidator<CreateTimePeriodicCredentialRequest>
{
    public CreateTimePeriodicCredentialRequestValidator()
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

        RuleFor(request => request.ExpirationTimeInSeconds)
            .NotEmpty()
            .WithMessage("Enter valid ExpirationTimeInSeconds")
            .NotNull()
            .WithMessage("Enter valid ExpirationTimeInSeconds")
            .LessThanOrEqualTo(0)
            .WithMessage("Enter valid ExpirationTimeInSeconds");
    }
}