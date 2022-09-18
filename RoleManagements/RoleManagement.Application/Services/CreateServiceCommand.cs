using RoleManagements.Domain;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record CreateServiceCommand(Name Name) : Command;