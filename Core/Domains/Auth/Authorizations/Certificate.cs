namespace Core.Domains.Auth.Authorizations;

public record Certificate(string Token, string RefreshToken)
{
    public override string ToString()
    {
        return $"Token: {Token}, RefreshToken: {RefreshToken}";
    }

    public virtual bool Equals(Certificate? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Token.Equals(other.Token);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Token);
    }
}