using Application.Scopes;
using Tes.CQRS;

namespace Data.Redis.Scopes;

internal sealed class ValidateScopeQueryHandler : QueryHandler<ValidateScopeQuery, bool>
{
    public override async Task<bool> HandleAsync(ValidateScopeQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}