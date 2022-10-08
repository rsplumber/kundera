using Domain.Roles;
using Domain.Scopes;
using Domain.Scopes.Types;
using Domain.Services;
using Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace Application.Scopes;

public sealed record ValidateScopeQuery(ScopeId ScopeId, ServiceId RequestedService, RoleId RequestedRole) : Query<bool>;