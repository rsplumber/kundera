using Application.Scopes;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Scopes;

public record CreateScopeRequest(string Name) : IWebRequest
{
    public CreateScopeCommand ToCommand() => new(Name);
}

public class CreateScopeRequestValidator : RequestValidator<CreateScopeRequest>
{
    public CreateScopeRequestValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(4)
            .WithMessage("Name minimum length is 4")
            .MaximumLength(30)
            .WithMessage("Name Maximum length is 30")
            .NotEmpty()
            .WithMessage("Enter a valid name")
            .NotNull()
            .WithMessage("Enter a valid name");
    }
}