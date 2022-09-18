using FluentValidation;
using RoleManagement.Application.Scopes;
using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Scopes.Types;
using Tes.Web.Validators;

namespace RoleManagement.Web.Api.Scopes;

public record AddScopeRoleRequest(List<string> RoleIds) : IWebRequest
{
    public AddScopeRoleCommand ToCommand(string scopeId)
    {
        var roles = RoleIds.Select(RoleId.From).ToArray();
        return new(ScopeId.From(scopeId), roles);
    }
}

public class AddScopeRoleRequestValidator : RequestValidator<AddScopeRoleRequest>
{
    public AddScopeRoleRequestValidator()
    {
        RuleFor(request => request.RoleIds)
            .NotEmpty().WithMessage("Enter valid Role")
            .NotNull().WithMessage("Enter valid Role");
    }
}

public record RemoveScopeRoleRequest(List<string> RoleIds) : IWebRequest
{
    public RemoveScopeRoleCommand ToCommand(string scopeId)
    {
        var roles = RoleIds.Select(RoleId.From).ToArray();
        return new(ScopeId.From(scopeId), roles);
    }
}

public class RemoveScopeRoleRequestValidator : RequestValidator<RemoveScopeRoleRequest>
{
    public RemoveScopeRoleRequestValidator()
    {
        RuleFor(request => request.RoleIds)
            .NotEmpty().WithMessage("Enter valid Role")
            .NotNull().WithMessage("Enter valid Role");
    }
}