using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Roles;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;

namespace Web.Api.Roles;

public record AddRolePermissionRequest(List<string> PermissionIds) : IWebRequest
{
    public AddRolePermissionCommand ToCommand(string roleId)
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

public record RemoveRolePermissionRequest(List<string> PermissionIds) : IWebRequest
{
    public RemoveRolePermissionCommand ToCommand(string roleId)
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