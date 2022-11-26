using ValueOf;

namespace Core.Domains.Scopes.Types;

public sealed class ScopeId : ValueOf<Guid, ScopeId>
{
    public static ScopeId Generate() => From(Guid.NewGuid());
}