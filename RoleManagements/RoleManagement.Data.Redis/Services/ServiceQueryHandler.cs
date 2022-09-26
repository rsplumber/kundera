using RoleManagement.Application.Services;
using Tes.CQRS;

namespace RoleManagement.Data.Redis.Services;

internal sealed class ServiceQueryHandler : QueryHandler<ServiceQuery, ServiceResponse>
{
    public override async Task<ServiceResponse> HandleAsync(ServiceQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}