using Application.Scopes;
using Domain.Roles;
using Domain.Scopes;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Scopes;

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