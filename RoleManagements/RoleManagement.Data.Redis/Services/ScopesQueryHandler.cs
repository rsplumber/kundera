using RoleManagement.Application.Services;
using Tes.CQRS;

namespace RoleManagement.Data.Redis.Services;

internal sealed class ServicesQueryHandler : QueryHandler<ServicesQuery, IEnumerable<ServicesResponse>>
{
    public override async Task<IEnumerable<ServicesResponse>> HandleAsync(ServicesQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}