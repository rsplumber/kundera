using Domain.Scopes;
using Domain.Scopes.Types;
using Domain.Services;
using Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace Application.Scopes;

public sealed record ValidateScopeServiceQuery(ScopeId ScopeId, ServiceId RequestedService) : Query<bool>;