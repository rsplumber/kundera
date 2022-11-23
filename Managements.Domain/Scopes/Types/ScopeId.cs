using ValueOf;

namespace Managements.Domain.Scopes.Types;

public sealed class ScopeId : ValueOf<Guid, ScopeId>
{
    public static ScopeId Generate() => From(Guid.NewGuid());
}