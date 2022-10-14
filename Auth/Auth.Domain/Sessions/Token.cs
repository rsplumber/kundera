using Kite.Domain.Contracts;

namespace Auth.Domain.Sessions;

public sealed record Token : IEntityIdentity
{
    private readonly string _value;

    private Token(string value)
    {
        _value = value;
    }

    public static Token From(string value) => new(value);

    public string Value => _value;

    public bool Equals(Token? other)
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