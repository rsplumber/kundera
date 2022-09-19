using Authentication.Domain.Exceptions;
using Authentication.Domain.Types;
using Tes.Domain.Contracts;

namespace Authentication.Domain;

public class Credential : AggregateRoot<UniqueIdentifier>
{
    private readonly Guid _userId;
    private readonly Password _password;

    protected Credential()
    {
    }

    private Credential(UniqueIdentifier uniqueIdentifier, Password password, UserId user) : base(uniqueIdentifier)
    {
        _userId = user;
        _password = password;
    }

    public static async Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier, Password password, UserId user, ICredentialRepository credentialRepository)
    {
        var exists = await credentialRepository.ExistsAsync(uniqueIdentifier);
        if (exists)
        {
            throw new DuplicateUniqueIdentifierException(uniqueIdentifier);
        }

        return new(uniqueIdentifier, password, user);
    }

    public UserId User => _userId;

    public Password Password => _password;
}