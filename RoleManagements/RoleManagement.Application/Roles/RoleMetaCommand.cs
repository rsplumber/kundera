using RoleManagements.Domain.Roles.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record AddRoleMetaCommand(RoleId RoleId, IDictionary<string, string> Meta) : Command;

public sealed record RemoveRoleMetaCommand(RoleId RoleId, IDictionary<string, string> Meta) : Command;