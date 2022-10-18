using Kite.CQRS.Contracts;

namespace Managements.Application.Services;

public sealed record ServicesQuery : Query<IEnumerable<ServicesResponse>>
{
    public string? Name { get; set; }
}

public sealed record ServicesResponse(Guid Id, string Name, string Status);