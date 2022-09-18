using FluentValidation;
using RoleManagement.Application.Roles;
using RoleManagements.Domain.Roles.Types;
using Tes.Web.Validators;

namespace RoleManagement.Web.Api.Roles;

public record AddRoleMetaRequest(Dictionary<string, string> Meta) : IWebRequest
{
    public AddRoleMetaCommand ToCommand(string roleId) => new(RoleId.From(roleId), Meta);
}

public class AddRoleMetaRequestValidator : RequestValidator<AddRoleMetaRequest>
{
    public AddRoleMetaRequestValidator()
    {
        RuleFor(request => request.Meta)
            .NotEmpty().WithMessage("Enter valid Meta")
            .NotNull().WithMessage("Enter valid Meta");
    }
}

public record RemoveRoleMetaRequest(Dictionary<string, string> Meta) : IWebRequest
{
    public RemoveRoleMetaCommand ToCommand(string roleId) => new(RoleId.From(roleId), Meta);
}

public class RemoveRoleMetaRequestValidator : RequestValidator<RemoveRoleMetaRequest>
{
    public RemoveRoleMetaRequestValidator()
    {
        RuleFor(request => request.Meta)
            .NotEmpty().WithMessage("Enter valid Meta")
            .NotNull().WithMessage("Enter valid Meta");
    }
}