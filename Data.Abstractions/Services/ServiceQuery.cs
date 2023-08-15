using Mediator;

namespace Data.Abstractions.Services;

public sealed record ServiceQuery : IQuery<ServiceResponse>
{
    public Guid ServiceId { get; init; } = default!;
}

public sealed record ServiceResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Secret { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;
}