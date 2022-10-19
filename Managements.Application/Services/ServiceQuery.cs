using Kite.CQRS.Contracts;
using Managements.Domain.Services;

namespace Managements.Application.Services;

public sealed record ServiceQuery(ServiceId Service) : Query<ServiceResponse>;

public sealed record ServiceResponse(Guid Id, string Name,string Secret, string Status);