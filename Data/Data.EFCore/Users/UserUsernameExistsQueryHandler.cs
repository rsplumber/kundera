using Core.Users.Exception;
using Data.Abstractions.Users;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Users;

public sealed class UserUsernameExistsQueryHandler : IQueryHandler<UserUsernameExistsQuery, UserUsernameExistsResponse>
{
    private readonly AppDbContext _dbContext;

    public UserUsernameExistsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<UserUsernameExistsResponse> Handle(UserUsernameExistsQuery query, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Usernames.Any(username => username == query.Username),
                cancellationToken);
        if (user is null) throw new UserNotFoundException();
        return new UserUsernameExistsResponse
        {
            Id = user.Id
        };
    }
}