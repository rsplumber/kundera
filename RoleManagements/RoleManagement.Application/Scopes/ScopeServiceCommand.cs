using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record AddScopeServiceCommand(params ServiceId[] Services) : Command;

public sealed record RemoveScopeServiceCommand(params ServiceId[] Services) : Command;