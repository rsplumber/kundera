using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Sessions;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Services;
using Core.Domains.Users;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth;

internal sealed class AuthorizeDataProvider : IAuthorizeDataProvider
{
    private readonly AppDbContext _dbContext;

    public AuthorizeDataProvider(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        return _dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.User)
            .Include(session => session.Activity)
            .Include(session => session.Scope)
            .ThenInclude(scope => scope.Roles)
            .FirstOrDefaultAsync(session => session.Id == sessionToken, cancellationToken);
    }

    public Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        return _dbContext.Groups.Include(u => u.Roles)
            .ThenInclude(role => role.Permissions)
            .SelectMany(group => group.Roles)
            .ToListAsync(cancellationToken);
    }

    public Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        return _dbContext.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(service => service.Secret == serviceSecret, cancellationToken: cancellationToken);
    }

}