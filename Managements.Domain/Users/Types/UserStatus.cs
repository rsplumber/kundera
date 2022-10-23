using Managements.Domain.Users.Exception;

namespace Managements.Domain.Users.Types;

public sealed record UserStatus
{
    public static readonly UserStatus Active = From(nameof(Active));
    public static readonly UserStatus Suspend = From(nameof(Suspend));
    public static readonly UserStatus Block = From(nameof(Block));

    private readonly string _value;

    private UserStatus(string value)
    {
        _value = value;
        Validate();
    }

    public static UserStatus From(string value) => new(value);

    private void Validate()
    {
        if (Value is not (nameof(Active) or nameof(Suspend) or nameof(Block)))
        {
            throw new UserStatusNotSupportedException(Value);
        }
    }

    public string Value => _value;

    public bool Equals(UserStatus? other)
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