using Kite.Domain.Contracts;

namespace Managements.Domain.Scopes;

public sealed record ScopeId : IEntityIdentity
{
    private readonly Guid _value;

    private ScopeId(Guid value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        _value = value;
    }

    public static ScopeId From(Guid value) => new(value);

    public static ScopeId Generate() => From(Guid.NewGuid());

    public Guid Value => _value;

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
        return _value.ToString();
    }
}