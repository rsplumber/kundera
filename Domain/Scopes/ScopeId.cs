using Tes.Domain.Contracts;

namespace Domain.Scopes;

public sealed record ScopeId : IIdentity
{
    private readonly string _value;

    private ScopeId(string value)
    {
        _value = value.Replace(" ", "").ToLower();
    }

    public static ScopeId From(string value) => new(value);

    public string Value => _value;

    public bool Equals(ScopeId? other)
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