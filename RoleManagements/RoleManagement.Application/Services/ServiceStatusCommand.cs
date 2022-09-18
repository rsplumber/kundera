using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record ActivateServiceCommand(ServiceId Id) : Command;

public sealed record DeActivateServiceCommand(ServiceId Id) : Command;