using Data.Abstractions.Scopes;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Scopes;

public sealed class ScopeSessionsQueryHandler : IQueryHandler<ScopeSessionsQuery, List<ScopeSessionResponse>>
{
    private readonly AppDbContext _dbContext;

    public ScopeSessionsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<ScopeSessionResponse>> Handle(ScopeSessionsQuery query, CancellationToken cancellationToken)
    {
        return await _dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.User)
            .Include(session => session.Scope)
            .Where(session => session.Scope.Id == query.ScopeId)
            .Select(model => new ScopeSessionResponse
            {
                Id = model.RefreshToken,
                UserId = model.User.Id,
                ScopeId = model.Scope.Id,
                ExpirationDateUtc = model.TokenExpirationDateUtc
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }
}