using Authorization.Domain.Types;

namespace Authorization.Application;

public record Certificate(Token Token, Token RefreshToken)
{
    public override string ToString()
    {
        return $"Token: {Token}, RefreshToken: {RefreshToken}";
    }

    public virtual bool Equals(Certificate? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Token.Equals(other.Token);
    }

    public override int GetHashCode()
    {
        return Token.GetHashCode();
    }
}