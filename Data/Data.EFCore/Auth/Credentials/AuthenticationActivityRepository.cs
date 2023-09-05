using Core.Auth.Credentials;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth.Credentials;

internal sealed class AuthenticationActivityRepository : IAuthenticationActivityRepository
{
    private readonly AppDbContext _dbContext;

    public AuthenticationActivityRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(AuthenticationActivity authenticationActivity, CancellationToken cancellationToken = default)
    {
        await _dbContext.AuthenticationActivities.AddAsync(authenticationActivity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

   
}