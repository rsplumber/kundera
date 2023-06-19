using Mediator;
using Microsoft.EntityFrameworkCore;
using Queries.Users;

namespace Data.Users;

public sealed class UserSessionsQueryHandler : IQueryHandler<UserSessionsQuery, List<UserSessionResponse>>
{
    private readonly AppDbContext _dbContext;


    public UserSessionsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<UserSessionResponse>> Handle(UserSessionsQuery query, CancellationToken cancellationToken)
    {
        return await _dbContext.Sessions
            .AsNoTracking()
            .Where(session  => session.User.Id == query.UserId)
            .Select(model => new UserSessionResponse
            {
                Id = model.RefreshToken,
                ExpirationDateUtc = model.ExpirationDateUtc,
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }
}