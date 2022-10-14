using Kite.CQRS.Contracts;

namespace Application.Services;

public sealed record ServicesQuery : Query<IEnumerable<ServicesResponse>>
{
    public string? Name { get; set; }
}

public sealed record ServicesResponse(string Id, string Status);