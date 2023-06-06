using Application.Permissions;
using Core.Domains.Services.Exceptions;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Permissions;

public sealed class PermissionsQueryHandler : IQueryHandler<PermissionsQuery, List<PermissionsResponse>>
{
    private readonly AppDbContext _dbContext;

    public PermissionsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<PermissionsResponse>> Handle(PermissionsQuery query, CancellationToken cancellationToken)
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