using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record DeActivateServiceCommand(ServiceId Service) : Command;