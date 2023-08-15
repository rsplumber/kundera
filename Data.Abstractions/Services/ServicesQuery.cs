using Mediator;

namespace Data.Abstractions.Services;

public sealed record ServicesQuery : PageableQuery, IQuery<PageableResponse<ServicesResponse>>
{
    public string? Name { get; set; }
}

public sealed record ServicesResponse(Guid Id, string Name, string Status);