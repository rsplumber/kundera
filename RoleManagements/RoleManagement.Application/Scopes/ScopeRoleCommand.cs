using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record AddScopeRoleCommand(ScopeId ScopeId, params RoleId[] Roles) : Command;

public sealed record RemoveScopeRoleCommand(ScopeId ScopeId, params RoleId[] Roles) : Command;