using Core.Domains.Services;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

internal class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _dbContext;


    public ServiceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Service entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Services.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Service?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Services
            .Include(service => service.Permissions)
            .FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    public Task<Service?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.Services
            .Include(service => service.Permissions)
            .FirstOrDefaultAsync(service => service.Name == name, cancellationToken);
    }

    public Task<Service?> FindBySecretAsync(string secret, CancellationToken cancellationToken = default)
    {
        return _dbContext.Services
            .Include(service => service.Permissions)
            .FirstOrDefaultAsync(service => service.Secret == secret, cancellationToken);
    }

    public async Task UpdateAsync(Service entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Services.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentService = await _dbContext.Services
            .FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
        if(currentService is null) return;
        _dbContext.Services.Remove(currentService);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}