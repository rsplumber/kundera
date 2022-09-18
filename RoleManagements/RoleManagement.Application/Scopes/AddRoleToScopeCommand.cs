using RoleManagements.Domain.Roles.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record AddRoleToScopeCommand(params RoleId[] Roles) : Command;