using RoleManagements.Domain.Roles.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record AddScopeRoleCommand(params RoleId[] Roles) : Command;

public sealed record RemoveScopeRoleCommand(params RoleId[] Roles) : Command;