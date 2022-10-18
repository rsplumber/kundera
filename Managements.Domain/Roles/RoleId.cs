using Kite.Domain.Contracts;

namespace Managements.Domain.Roles;

public sealed record RoleId : IEntityIdentity
{
    private readonly Guid _value;

    private RoleId(Guid value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        _value = value;
    }

    public static RoleId From(Guid value) => new(value);

    public static RoleId Generate() => From(Guid.NewGuid());

    public Guid Value => _value;

    public bool Equals(RoleId? other)
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