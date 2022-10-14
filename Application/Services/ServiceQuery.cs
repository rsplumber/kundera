using Domain.Services;
using Kite.CQRS.Contracts;

namespace Application.Services;

public sealed record ServiceQuery(ServiceId Service) : Query<ServiceResponse>;

public sealed record ServiceResponse(string Id, string Status);