using Tes.Domain.Contracts;

namespace Auth.Domain.Credentials;

public record Password : ValueObject
{
    private readonly string _value;
    private string _salt = string.Empty;

    private Password(string value, string salt)
    {
        _value = value;
        _salt = salt;
    }

    public static Password From(string value)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashed = BCrypt.Net.BCrypt.HashPassword(value, salt);
        return new Password(hashed, salt);
    }

    public static Password From(string value, string salt)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(value, salt);
        return new Password(hashed, salt);
    }

    public bool Verify(string value)
    {
        return BCrypt.Net.BCrypt.Verify(value , _value);
    }

    public string Value => _value;

    public string Salt => _salt;

    public virtual bool Equals(Password? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && _value == other._value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_value);
    }

    public override string ToString()
    {
        return _value;
    }
}