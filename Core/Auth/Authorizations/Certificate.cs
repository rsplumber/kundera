using Core.Auth.Credentials;
using Core.Hashing;

namespace Core.Auth.Authorizations;

public sealed record Certificate
{
    public static Certificate Create(IHashService hashService, Credential credential, Guid scopeId)
    {
        var token = hashService.Hash(credential.User.Id.ToString(), scopeId.ToString());
        var refreshToken = hashService.Hash(new Random().RandomCharsAndNumbers(6));
        var expireTime = (double)(credential.SessionExpireTimeInMinutes ?? 0);
        return new Certificate(token, refreshToken)
        {
            ExpireAtUtc = DateTime.UtcNow.AddMinutes(expireTime)
        };
    }

    private Certificate(string token, string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }

    public DateTime ExpireAtUtc { get; init; }

    public string RefreshToken { get; init; } = default!;

    public string Token { get; init; } = default!;

    private sealed class ExpireAtUtcRefreshTokenTokenEqualityComparer : IEqualityComparer<Certificate>
    {
        public bool Equals(Certificate? x, Certificate? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.ExpireAtUtc.Equals(y.ExpireAtUtc) && x.RefreshToken == y.RefreshToken && x.Token == y.Token;
        }

        public int GetHashCode(Certificate obj)
        {
            return HashCode.Combine(obj.ExpireAtUtc, obj.RefreshToken, obj.Token);
        }
    }

    public static IEqualityComparer<Certificate> ExpireAtUtcRefreshTokenTokenComparer { get; } = new ExpireAtUtcRefreshTokenTokenEqualityComparer();

    public bool Equals(Certificate? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ExpireAtUtc.Equals(other.ExpireAtUtc) && RefreshToken == other.RefreshToken && Token == other.Token;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ExpireAtUtc, RefreshToken, Token);
    }

    public override string ToString()
    {
        return $"{nameof(ExpireAtUtc)}: {ExpireAtUtc}, {nameof(RefreshToken)}: {RefreshToken}, {nameof(Token)}: {Token}";
    }
}