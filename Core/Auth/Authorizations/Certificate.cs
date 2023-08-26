using Core.Auth.Credentials;
using Core.Hashing;

namespace Core.Auth.Authorizations;

public sealed record Certificate
{
    public static Certificate Create(IHashService hashService, Credential credential, Guid scopeId)
    {
        var hashKey = Random.Shared.RandomCharsAndNumbers(6);
        var token = hashService.HashAsync(hashKey, credential.User.Id.ToString(), scopeId.ToString()).Result;
        var refreshToken = hashService.HashAsync(hashKey, Random.Shared.RandomCharsAndNumbers(6)).Result;
        var expireTime = (double)(credential.SessionTokenExpireTimeInMinutes ?? 0);
        return new Certificate(token, refreshToken)
        {
            ExpireAtUtc = DateTime.UtcNow.AddMinutes(expireTime)
        };
    }

    public static Certificate From(string token, string refreshToken) => new(token, refreshToken);

    private Certificate(string token, string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }

    public DateTime ExpireAtUtc { get; init; }

    public string RefreshToken { get; init; } = default!;

    public string Token { get; init; } = default!;

    public bool Equals(Certificate? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return RefreshToken == other.RefreshToken && Token == other.Token;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RefreshToken, Token);
    }

    public override string ToString()
    {
        return $"{nameof(ExpireAtUtc)}: {ExpireAtUtc}, {nameof(RefreshToken)}: {RefreshToken}, {nameof(Token)}: {Token}";
    }
}