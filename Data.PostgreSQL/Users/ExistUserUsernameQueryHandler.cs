using Mediator;
using Microsoft.EntityFrameworkCore;
using Queries.Users;

namespace Data.Users;

public sealed class ExistUserUsernameQueryHandler : IQueryHandler<ExistUserUsernameQuery, bool>
{
    private readonly AppDbContext _dbContext;

    public ExistUserUsernameQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<bool> Handle(ExistUserUsernameQuery query, CancellationToken cancellationToken)
    {
        return  await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Usernames.Any(username => username == query.Username),
                cancellationToken);;
    }
}