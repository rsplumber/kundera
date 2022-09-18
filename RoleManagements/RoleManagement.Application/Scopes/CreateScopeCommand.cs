using RoleManagements.Domain;
using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record CreateScopeCommand(Name Name, IList<ServiceId> Services, IList<RoleId> Roles) : Command;