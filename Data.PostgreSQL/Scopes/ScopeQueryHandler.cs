using Core.Permissions.Exceptions;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Queries.Scopes;

namespace Data.Scopes;

public sealed class ScopeQueryHandler : IQueryHandler<ScopeQuery, ScopeResponse>
{
    private readonly AppDbContext _dbContext;

    public ScopeQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<ScopeResponse> Handle(ScopeQuery query, CancellationToken cancellationToken)
    {
        var scope = await _dbContext.Scopes
            .AsNoTracking()
            .Include(scope =>scope.Roles )
            .Include(scope =>scope.Services )
            .FirstOrDefaultAsync(s => s.Id == query.ScopeId, cancellationToken: cancellationToken);
        
        if (scope is null)
        {
            throw new PermissionNotFoundException();
        }

        return new ScopeResponse
        {
            Id = scope.Id,
            Name = scope.Name,
            Secret = scope.Secret,
            Status = scope.Status.ToString(),
            Roles = scope.Roles.Select(role => role.Id),
            Services = scope.Services.Select(service => service.Id)
        };
    }
}