using Kite.Domain.Contracts;

namespace Managements.Domain.Groups;

public sealed record GroupId : IEntityIdentity
{
    private readonly Guid _value;

    private GroupId(Guid value)
    {
        _value = value;
    }

    public static GroupId From(Guid value) => new(value);

    public static GroupId Generate() => From(Guid.NewGuid());

    public Guid Value => _value;

    public bool Equals(GroupId? other)
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