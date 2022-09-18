using FluentValidation;
using RoleManagement.Application.Roles;
using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles.Types;
using Tes.Web.Validators;

namespace RoleManagement.Web.Api.Roles;

public record AddRolePermissionRequest(List<string> PermissionIds) : IWebRequest
{
    public AddRolePermissionCommand ToCommand(string roleId)
    {
        var permissions = PermissionIds.Select(PermissionId.From).ToArray();
        return new(RoleId.From(roleId), permissions);
    }
}

public class AddRolePermissionRequestValidator : RequestValidator<AddRolePermissionRequest>
{
    public AddRolePermissionRequestValidator()
    {
        RuleFor(request => request.PermissionIds)
            .NotEmpty().WithMessage("Enter valid Permission")
            .NotNull().WithMessage("Enter valid Permission");
    }
}

public record RemoveRolePermissionRequest(List<string> PermissionIds) : IWebRequest
{
    public RemoveRolePermissionCommand ToCommand(string roleId)
    {
        var permissions = PermissionIds.Select(PermissionId.From).ToArray();
        return new(RoleId.From(roleId), permissions);
    }
}

public class RemoveRolePermissionRequestValidator : RequestValidator<RemoveRolePermissionRequest>
{
    public RemoveRolePermissionRequestValidator()
    {
        RuleFor(request => request.PermissionIds)
            .NotEmpty().WithMessage("Enter valid Permission")
            .NotNull().WithMessage("Enter valid Permission");
    }
}