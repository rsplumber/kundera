using Application.Users;
using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.PostgreSQL.Users;

public sealed class UserQueryHandler : IQueryHandler<UserQuery, UserResponse>
{
    private readonly AppDbContext _dbContext;

    public UserQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<UserResponse> Handle(UserQuery query, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Roles)
            .Include(user => user.Groups)
            .FirstOrDefaultAsync(user => user.Id == query.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return new UserResponse
        {
            Id = user.Id,
            Usernames = user.Usernames.ToList(),
            Status = user.Status.ToString(),
            Roles = user.Roles.Select(role => role.Id),
            Groups = user.Groups.Select(group => group.Id),
        };
    }
}