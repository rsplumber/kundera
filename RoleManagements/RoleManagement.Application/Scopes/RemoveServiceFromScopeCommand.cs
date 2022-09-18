using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record RemoveServiceFromScopeCommand(params ServiceId[] Services) : Command;