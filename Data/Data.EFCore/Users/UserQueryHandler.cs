﻿using Core.Users.Exception;
using Data.Abstractions.Users;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Users;

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
            Status = user.Status.ToString(),
            Roles = user.Roles.Select(role => role.Id),
            Groups = user.Groups.Select(group => group.Id),
        };
    }
}