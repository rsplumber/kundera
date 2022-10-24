namespace Managements.Domain.Scopes.Types;

public sealed record ScopeSecret
{
    private readonly string _value;

    private ScopeSecret(string value)
    {
        _value = value;
    }

    public static ScopeSecret From(string value) => new(value);

    public static implicit operator string(ScopeSecret secret) => secret.Value;

    public string Value => _value;

    public bool Equals(ScopeSecret? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }
}