using Application.Roles;
using Application.Scopes;
using Core.Domains.Scopes.Exceptions;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.PostgreSQL.Scopes;

public sealed class ScopeRolesQueryHandler : IQueryHandler<ScopeRolesQuery, List<ScopeRolesResponse>>
{
    private readonly AppDbContext _dbContext;


    public ScopeRolesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<ScopeRolesResponse>> Handle(ScopeRolesQuery query, CancellationToken cancellationToken)
    {
        return await _dbContext.Scopes
            .AsNoTracking()
            .Where(scope => scope.Id == query.ScopeId)
            .Include(scope => scope.Roles)
            .SelectMany(scope => scope.Roles.Select(role => new ScopeRolesResponse(role.Id, role.Name)))
            .ToListAsync( cancellationToken: cancellationToken);
    }
}