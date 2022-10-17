namespace Managements.Domain.Users;

public record Username
{
    private readonly string _value;

    private Username(string value)
    {
        _value = value;
    }

    public static Username From(string username) => new(username);

    public static implicit operator string(Username username) => username.Value;

    public static implicit operator Username(string name) => new(name);

    public string Value => _value;

    public virtual bool Equals(Username? other)
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