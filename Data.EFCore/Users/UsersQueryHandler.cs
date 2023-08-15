using Data.Abstractions;
using Data.Abstractions.Users;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Users;

public sealed class UsersQueryHandler : IQueryHandler<UsersQuery, PageableResponse<UsersResponse>>
{
    private readonly AppDbContext _dbContext;

    public UsersQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PageableResponse<UsersResponse>> Handle(UsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
            .Page(query)
            .Select(model => new UsersResponse(model.Id, model.Usernames))
            .ToListAsync(cancellationToken: cancellationToken);

        var counts = await _dbContext.Users.CountAsync(cancellationToken);

        return new PageableResponse<UsersResponse>
        {
            Data = users,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}