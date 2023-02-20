using ValueOf;

namespace Core.Domains.Auth.Credentials;

public sealed class CredentialId : ValueOf<Guid, CredentialId>
{
    public static CredentialId Generate() => From(Guid.NewGuid());
}