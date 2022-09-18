using FluentValidation;
using RoleManagement.Application.Permissions;
using RoleManagements.Domain.Permissions.Types;
using Tes.Web.Validators;

namespace RoleManagement.Web.Api.Permissions;

public record AddPermissionMetaRequest(Dictionary<string, string> Meta) : IWebRequest
{
    public AddPermissionMetaCommand ToCommand(string permissionId) => new(PermissionId.From(permissionId), Meta);
}

public class AddPermissionMetaRequestValidator : RequestValidator<AddPermissionMetaRequest>
{
    public AddPermissionMetaRequestValidator()
    {
        RuleFor(request => request.Meta)
            .NotEmpty().WithMessage("Enter valid Meta")
            .NotNull().WithMessage("Enter valid Meta");
    }
}

public record RemovePermissionMetaRequest(Dictionary<string, string> Meta) : IWebRequest
{
    public RemovePermissionMetaCommand ToCommand(string permissionId) => new(PermissionId.From(permissionId), Meta);
}

public class RemovePermissionMetaRequestValidator : RequestValidator<RemovePermissionMetaRequest>
{
    public RemovePermissionMetaRequestValidator()
    {
        RuleFor(request => request.Meta)
            .NotEmpty().WithMessage("Enter valid Meta")
            .NotNull().WithMessage("Enter valid Meta");
    }
}