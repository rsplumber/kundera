using Application.Roles;
using Tes.CQRS;

namespace Data.Redis.Roles;

internal sealed class ValidateRoleQueryHandler : QueryHandler<ValidateRoleQuery, bool>
{
    public override async Task<bool> HandleAsync(ValidateRoleQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}