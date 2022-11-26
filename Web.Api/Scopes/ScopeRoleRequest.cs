﻿using Application.Scopes;
using Core.Domains.Roles.Types;
using Core.Domains.Scopes.Types;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Scopes;

public record AddScopeRoleRequest(List<Guid> RoleIds) : IWebRequest
{
    public AddScopeRoleCommand ToCommand(Guid scopeId)
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

public record RemoveScopeRoleRequest(List<Guid> RoleIds) : IWebRequest
{
    public RemoveScopeRoleCommand ToCommand(Guid scopeId)
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