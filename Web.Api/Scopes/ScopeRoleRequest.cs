using FluentValidation;
using Kite.Web.Requests;
using Managements.Application.Scopes;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;

namespace Web.Apix.Scopes;

public record AddScopeRoleRequest(List<string> RoleIds) : IWebRequest
{
    public AddScopeRoleCommand ToCommand(string scopeId)
    {
        var roles = RoleIds.Select(RoleId.From)
                           .ToArray();

        return new(ScopeId.From(scopeId), roles);
    }
}

public class AddScopeRoleRequestValidator : RequestValidator<AddScopeRoleRequest>
{
    public AddScopeRoleRequestValidator()
    {
        RuleFor(request => request.RoleIds)
            .NotEmpty()
            .WithMessage("Enter valid Role")
            .NotNull()
            .WithMessage("Enter valid Role");
    }
}

public record RemoveScopeRoleRequest(List<string> RoleIds) : IWebRequest
{
    public RemoveScopeRoleCommand ToCommand(string scopeId)
    {
        var roles = RoleIds.Select(RoleId.From)
                           .ToArray();

        return new(ScopeId.From(scopeId), roles);
    }
}

public class RemoveScopeRoleRequestValidator : RequestValidator<RemoveScopeRoleRequest>
{
    public RemoveScopeRoleRequestValidator()
    {
        RuleFor(request => request.RoleIds)
            .NotEmpty()
            .WithMessage("Enter valid Role")
            .NotNull()
            .WithMessage("Enter valid Role");
    }
}