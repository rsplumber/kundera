using Core.Services.Exceptions;
using Data.Abstractions.Services;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public sealed class ServicePermissionsQueryHandler : IQueryHandler<ServicePermissionsQuery, List<PermissionsResponse>>
{
    private readonly AppDbContext _dbContext;

    public ServicePermissionsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<PermissionsResponse>> Handle(ServicePermissionsQuery query, CancellationToken cancellationToken)
    {
        var selectedService = await _dbContext.Services
            .AsNoTracking()
            .Include(service => service.Permissions)
            .FirstOrDefaultAsync(s => s.Id == query.ServiceId, cancellationToken);

        if (selectedService is null)
        {
            throw new ServiceNotFoundException();
        }

        var permissions = selectedService.Permissions;
        if (query.Name is not null)
        {
            permissions = permissions.Where(model => model.Name.Contains(query.Name)).ToList();
        }

        return permissions
            .Select(model => new PermissionsResponse(model.Id, model.Name))
            .ToList();
    }
}