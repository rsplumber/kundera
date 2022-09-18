using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record ServicesQuery(ServiceId ServiceId) : Query<ServicesResponse>;

public sealed record ServicesResponse(string Id, string Status);