using Kite.Domain.Contracts;

namespace Managements.Domain.Permissions;

public sealed record PermissionId : IEntityIdentity
{
    private readonly Guid _value;

    private PermissionId(Guid value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        _value = value;
    }

    public static PermissionId From(Guid value) => new(value);

    public static PermissionId Generate() => From(Guid.NewGuid());


    public Guid Value => _value;

    public bool Equals(PermissionId? other)
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