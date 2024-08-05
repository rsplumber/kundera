using Core.Auth.Authorizations;
using Core.Users.Exception;
using Data.Abstractions.Users;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Users;

public sealed class UserRoleQueryHandler : IQueryHandler<UsersRolesQuery, List<UserRoleResponse>>
{
    private readonly AppDbContext _dbContext;
    private readonly IAuthorizeDataProvider _authorizeDataProvider;


    public UserRoleQueryHandler(AppDbContext dbContext, IAuthorizeDataProvider authorizeDataProvider)
    {
        _dbContext = dbContext;
        _authorizeDataProvider = authorizeDataProvider;
    }

    public async ValueTask<List<UserRoleResponse>> Handle(UsersRolesQuery query, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Groups)
            .Include(user => user.Roles)
            .ThenInclude(role => role.Permissions)
            .Where(user => user.Id == query.UserId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user is null) throw new UserNotFoundException();
        var userRoles = await _authorizeDataProvider.FindUserRolesAsync(user, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return userRoles.Select(role => new UserRoleResponse
        {
            Role = role.Name,
            Permissions = role.Permissions.Select(permission => permission.Name).ToList()
        }).ToList();
    }
}