using System.Security.Cryptography;
using System.Text;
using Core.Extensions;

namespace Core.Hashing;

internal sealed class HmacHashingService : IHashService
{
    private readonly HashingType _hashingType;
    private readonly int _randomKeyLength;
    private readonly Random _random;

    public HmacHashingService(HashingType hashingType = HashingType.HMACSHA384, int randomKeyLength = 8)
    {
        _hashingType = hashingType;
        _randomKeyLength = randomKeyLength;
        _random = new();
    }

    public string Hash(params string[] values)
    {
        var combinedValues = string.Concat(values);
        var combinedValuesBytes = Encoding.UTF8.GetBytes(combinedValues);

        var hashedBytes = GetHasher(_hashingType)
            .ComputeHash(combinedValuesBytes);

        return Convert.ToHexString(hashedBytes);
    }


    private HMAC GetHasher(HashingType type)
    {
        var random = _random.RandomCharsAndNumbers(_randomKeyLength);
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