using Core.Auth.Credentials.Exceptions;
using Data.Abstractions.Users;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Users;

public sealed class UserUsernameCredentialQueryHandler : IQueryHandler<UserUsernameCredentialQuery, UserUsernameCredentialQueryResponse>
{
    private readonly AppDbContext _dbContext;

    public UserUsernameCredentialQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<UserUsernameCredentialQueryResponse> Handle(UserUsernameCredentialQuery query, CancellationToken cancellationToken)
    {
        var credential = await _dbContext.Credentials
            .Include(c => c.User)
            .FirstOrDefaultAsync(credential => credential.Username == query.Username, cancellationToken: cancellationToken);
        if (credential is null) throw new CredentialNotFoundException();
        return new UserUsernameCredentialQueryResponse
        {
            Username = credential.Username,
            Id = credential.Id,
            UserId = credential.User.Id
        };
    }
}