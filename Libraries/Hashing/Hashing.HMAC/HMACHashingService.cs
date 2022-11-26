using System.Security.Cryptography;
using System.Text;
using Kite.Extensions.Random;
using Kite.Hashing;

namespace Hashing.HMAC;

public sealed class HMACHashingService : IHashService
{
    private readonly HashingType _hashingType;
    private readonly int _randomKeyLength;
    private static readonly Random Random = new();

    public HMACHashingService(HashingType hashingType = HashingType.HMACSHA384, int randomKeyLength = 8)
    {
        _hashingType = hashingType;
        _randomKeyLength = randomKeyLength;
    }

    public string Hash(params string[] values)
    {
        var combinedValues = string.Concat(values);
        var combinedValuesBytes = Encoding.UTF8.GetBytes(combinedValues);

        var hashedBytes = GetHasher(_hashingType)
            .ComputeHash(combinedValuesBytes);

        return Convert.ToHexString(hashedBytes);
    }


    private System.Security.Cryptography.HMAC GetHasher(HashingType type)
    {
        var random = Random.RandomCharsNums(_randomKeyLength);
        var randomBytes = Encoding.UTF8.GetBytes(random);

        return type switch
        {
            HashingType.HMACMD5 => new HMACMD5(randomBytes),
            HashingType.HMACSHA1 => new HMACSHA1(randomBytes),
            HashingType.HMACSHA256 => new HMACSHA256(randomBytes),
            HashingType.HMACSHA384 => new HMACSHA384(randomBytes),
            HashingType.HMACSHA512 => new HMACSHA512(randomBytes),
            _ => new HMACSHA384(randomBytes)
        };
    }
}