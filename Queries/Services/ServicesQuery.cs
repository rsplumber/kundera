using Mediator;

namespace Queries.Services;

public sealed record ServicesQuery : IQuery<List<ServicesResponse>>
{
    public string? Name { get; set; }
}

public sealed record ServicesResponse(Guid Id, string Name, string Status);