using RoleManagements.Domain;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record CreateScopeCommand(Name Name) : Command;