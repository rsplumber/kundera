using Application.Users;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.PostgreSQL.Users;

public sealed class UsersQueryHandler : IQueryHandler<UsersQuery, List<UsersResponse>>
{
    private readonly AppDbContext _dbContext;

    public UsersQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<UsersResponse>> Handle(UsersQuery query, CancellationToken cancellationToken)
    {
        return  await _dbContext.Users
            .Select(model => new UsersResponse(model.Id, model.Usernames))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}