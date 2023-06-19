using Core.Services.Exceptions;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Queries.Services;

namespace Data.Services;

public sealed class ServiceQueryHandler : IQueryHandler<ServiceQuery, ServiceResponse>
{
    private readonly AppDbContext _dbContext;

    public ServiceQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<ServiceResponse> Handle(ServiceQuery query, CancellationToken cancellationToken)
    {
        var service = await _dbContext.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == query.ServiceId, cancellationToken: cancellationToken);
        
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }
        
        return new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Secret = service.Secret,
            Status = service.Status.ToString()
        };
    }
}