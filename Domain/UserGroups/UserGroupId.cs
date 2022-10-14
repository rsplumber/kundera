using Kite.Domain.Contracts;

namespace Domain.UserGroups;

public sealed record UserGroupId : IEntityIdentity
{
    private readonly Guid _value;

    private UserGroupId(Guid value)
    {
        _value = value;
    }

    public static UserGroupId From(Guid value) => new(value);

    public static UserGroupId Generate() => From(Guid.NewGuid());

    public Guid Value => _value;

    public bool Equals(UserGroupId? other)
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