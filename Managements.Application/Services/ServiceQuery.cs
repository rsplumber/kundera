using Kite.CQRS.Contracts;
using Managements.Domain.Services;

namespace Managements.Application.Services;

public sealed record ServiceQuery(ServiceId Service) : Query<ServiceResponse>;

public sealed record ServiceResponse(string Id, string Status);