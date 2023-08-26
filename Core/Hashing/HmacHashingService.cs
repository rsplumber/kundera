using System.Security.Cryptography;
using System.Text;

namespace Core.Hashing;

internal sealed class HmacHashingService : IHashService
{
    private readonly HashingType _hashingType;

    public HmacHashingService(HashingType hashingType = HashingType.HMACSHA384)
    {
        _hashingType = hashingType;
    }

    public Task<string> HashAsync(string key, params string[] values)
    {
        var combinedValues = string.Concat(values);
        var combinedValuesBytes = Encoding.UTF8.GetBytes(combinedValues);
        var hashedBytes = GetHasher(key, _hashingType)
            .ComputeHash(combinedValuesBytes);
        return Task.FromResult(Convert.ToHexString(hashedBytes));
    }

    private static HMAC GetHasher(string key, HashingType type)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        return type switch
        {
            HashingType.HMACMD5 => new HMACMD5(keyBytes),
            HashingType.HMACSHA1 => new HMACSHA1(keyBytes),
            HashingType.HMACSHA256 => new HMACSHA256(keyBytes),
            HashingType.HMACSHA384 => new HMACSHA384(keyBytes),
            HashingType.HMACSHA512 => new HMACSHA512(keyBytes),
            _ => new HMACSHA384(keyBytes)
        };
    }
}