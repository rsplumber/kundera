using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Scopes.Types;
using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record ValidateScopeQuery(ScopeId ScopeId, ServiceId RequestedService, RoleId RequestedRole) : Query<bool>;

public sealed record ValidateScopeRoleQuery(ScopeId ScopeId, RoleId RequestedRole) : Query<bool>;

public sealed record ValidateScopeServiceQuery(ScopeId ScopeId, ServiceId RequestedService) : Query<bool>;