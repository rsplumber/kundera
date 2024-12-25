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
        var authenticateActivities = await _dbContext.AuthenticationActivities
            .Where(activity => activity.UserId == query.UserId)
            .OrderByDescending(response => response.CreatedDateUtc)
            .Page(query)
            .Select(model => new UserAuthenticateActivitiesResponse()
            {
                Id = model.Id,
                Agent = model.Agent,
                UserId = model.UserId,
                IpAddress = model.IpAddress,
                CreatedDateUtc = model.CreatedDateUtc,
            })
            .ToListAsync(cancellationToken: cancellationToken);
        var counts = authenticateActivities.Count;

        return new PageableResponse<UserAuthenticateActivitiesResponse>
        {
            Data = authenticateActivities,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}