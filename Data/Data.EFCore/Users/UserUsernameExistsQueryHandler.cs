using Core.Auth.Credentials;
using Core.Users.Exception;
using Data.Abstractions.Users;
using Mediator;

namespace Data.Users;

public sealed class UserUsernameExistsQueryHandler : IQueryHandler<UserUsernameExistsQuery, UserUsernameExistsResponse>
{
    private readonly ICredentialRepository _credentialRepository;

    public UserUsernameExistsQueryHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }


    public async ValueTask<UserUsernameExistsResponse> Handle(UserUsernameExistsQuery query, CancellationToken cancellationToken)
    {
        var currentCredentials = await _credentialRepository.FindByUsernameAsync(query.Username, cancellationToken);
        if (currentCredentials.Count == 0) throw new UserNotFoundException();
        return new UserUsernameExistsResponse
        {
            Id = currentCredentials.First().User.Id
        };
    }
}