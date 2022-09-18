using RoleManagements.Domain.Services.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record ServiceQuery(ServiceId ServiceId) : Query<ServiceResponse>;

public sealed record ServiceResponse(string Id, string Status);