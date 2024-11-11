using Data.Abstractions;
using Data.Abstractions.Users;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Users;

public sealed class UserAuthenticateActivitiesQueryHandler : IQueryHandler<UserAuthenticateActivitiesQuery, PageableResponse<UserAuthenticateActivitiesResponse>>
{
    private readonly AppDbContext _dbContext;

    public UserAuthenticateActivitiesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PageableResponse<UserAuthenticateActivitiesResponse>> Handle(UserAuthenticateActivitiesQuery query, CancellationToken cancellationToken)
    {
        var users = await _dbContext.AuthenticationActivities
            .Page(query)
            .Select(model => new UserAuthenticateActivitiesResponse()
            {
                Id = model.Id,
                Agent = model.Agent,
                Username = model.Username,
                UserId = model.UserId,
                IpAddress = model.IpAddress,
                CreatedDateUtc = model.CreatedDateUtc,
            })
            .ToListAsync(cancellationToken: cancellationToken);
        var counts = await _dbContext.Users.CountAsync(cancellationToken);

        return new PageableResponse<UserAuthenticateActivitiesResponse>
        {
            Data = users,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}