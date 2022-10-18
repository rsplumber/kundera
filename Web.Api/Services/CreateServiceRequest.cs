using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Services;

namespace Web.Api.Services;

public record CreateServiceRequest(string Name) : IWebRequest
{
    public CreateServiceCommand ToCommand() => new(Name);
}

public class CreateServiceRequestValidator : RequestValidator<CreateServiceRequest>
{
    public CreateServiceRequestValidator()
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