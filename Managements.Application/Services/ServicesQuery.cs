using Mediator;

namespace Managements.Application.Services;

public sealed record ServicesQuery : IQuery<IEnumerable<ServicesResponse>>
{
    public string? Name { get; set; }
}

public sealed record ServicesResponse(Guid Id, string Name, string Status);