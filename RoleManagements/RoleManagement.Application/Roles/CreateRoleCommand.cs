using RoleManagements.Domain;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record CreateRoleCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;