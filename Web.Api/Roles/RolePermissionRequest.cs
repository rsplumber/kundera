using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Roles;
using Managements.Domain.Permissions;
using Managements.Domain.Permissions.Types;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Types;

namespace Web.Api.Roles;

public record AddRolePermissionRequest(List<Guid> PermissionIds) : IWebRequest
{
    public AddRolePermissionCommand ToCommand(Guid roleId)
    {
        var permissions = PermissionIds.Select(PermissionId.From)
            .ToArray();

        return new(RoleId.From(roleId), permissions);
    }
}

public class AddRolePermissionRequestValidator : RequestValidator<AddRolePermissionRequest>
{
    public AddRolePermissionRequestValidator()
    {
        RuleFor(request => request.PermissionIds)
            .NotEmpty()
            .WithMessage("Enter valid Permission")
            .NotNull()
            .WithMessage("Enter valid Permission");
    }
}

public record RemoveRolePermissionRequest(List<Guid> PermissionIds) : IWebRequest
{
    public RemoveRolePermissionCommand ToCommand(Guid roleId)
    {
        var permissions = PermissionIds.Select(PermissionId.From)
            .ToArray();

        return new(RoleId.From(roleId), permissions);
    }
}

public class RemoveRolePermissionRequestValidator : RequestValidator<RemoveRolePermissionRequest>
{
    public RemoveRolePermissionRequestValidator()
    {
        RuleFor(request => request.PermissionIds)
            .NotEmpty()
            .WithMessage("Enter valid Permission")
            .NotNull()
            .WithMessage("Enter valid Permission");
    }
}