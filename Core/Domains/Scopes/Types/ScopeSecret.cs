using ValueOf;

namespace Core.Domains.Scopes.Types;

public sealed class ScopeSecret : ValueOf<string, ScopeSecret>
{
    public static implicit operator string(ScopeSecret secret) => secret.Value;
}