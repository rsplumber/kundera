using Application.Permissions;
using Application.Roles;
using FluentValidation;
using Tes.Web.Validators;
using Web.Api.Roles;

namespace Web.Api.Permissions;

public record CreatePermissionRequest(string Name) : IWebRequest
{
    public Dictionary<string, string>? Meta { get; set; }

    public CreatePermissionCommand ToCommand() => new(Name, Meta);
}

public class CreatePermissionRequestValidator : RequestValidator<CreatePermissionRequest>
{
    public CreatePermissionRequestValidator()
    {
        RuleFor(request => request.Name)
            .MinimumLength(4).WithMessage("Name minimum length is 4")
            .MaximumLength(30).WithMessage("Name Maximum length is 30")
            .NotEmpty().WithMessage("Enter a valid name")
            .NotNull().WithMessage("Enter a valid name");
    }
}