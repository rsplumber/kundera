using RoleManagements.Domain.Roles.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record AddRoleMetaCommand(RoleId Role, IDictionary<string, string> Meta) : Command;