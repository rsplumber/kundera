using RoleManagements.Domain.Scopes.Types;
using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record AddScopeServiceCommand(ScopeId ScopeId, params ServiceId[] Services) : Command;

public sealed record RemoveScopeServiceCommand(ScopeId ScopeId, params ServiceId[] Services) : Command;