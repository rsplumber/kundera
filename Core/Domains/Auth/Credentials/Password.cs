namespace Core.Domains.Auth.Credentials;

public record Password
{
    private Password(string value, string salt)
    {
        Value = value;
        Salt = salt;
    }

    public static Password From(string value, string salt) => new(value, salt);

    public static Password Create(string value)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashed = BCrypt.Net.BCrypt.HashPassword(value, salt);
        return new(hashed, salt);
    }

    public static Password Create(string value, string salt)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(value, salt);
        return new(hashed, salt);
    }

    public string Value { get; }

    public string Salt { get; } = string.Empty;

    public bool Verify(string value)
    {
        return BCrypt.Net.BCrypt.Verify(value, Value);
    }

    public bool Check(string password) => Equals(Create(password, Salt));


    public virtual bool Equals(Password? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public override string ToString()
    {
        return Value;
    }
}