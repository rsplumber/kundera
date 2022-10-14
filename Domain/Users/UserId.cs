using Kite.Domain.Contracts;

namespace Domain.Users;

public sealed record UserId : IEntityIdentity
{
    private readonly Guid _value;

    private UserId(Guid value)
    {
        _value = value;
    }

    public static UserId From(Guid value) => new(value);

    public static UserId Generate() => From(Guid.NewGuid());

    public Guid Value => _value;

    public bool Equals(UserId? other)
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