using Domain.Roles;
using Domain.Scopes;
using Domain.Scopes.Types;
using Tes.CQRS.Contracts;

namespace Application.Scopes;

public sealed record ValidateScopeRoleQuery(ScopeId ScopeId, RoleId RequestedRole) : Query<bool>;