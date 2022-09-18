using FluentValidation;
using RoleManagement.Application.Roles;
using Tes.Web.Validators;

namespace RoleManagement.Web.Api.Roles;

public record CreateRoleRequest(string Name) : IWebRequest
{
    public Dictionary<string, string>? Meta { get; set; }

    public CreateRoleCommand ToCommand() => new(Name, Meta);
}

public class CreateRoleRequestValidator : RequestValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(4).WithMessage("Name minimum length is 4")
            .MaximumLength(30).WithMessage("Name Maximum length is 30")
            .NotEmpty().WithMessage("Enter a valid name")
            .NotNull().WithMessage("Enter a valid name");
    }
}