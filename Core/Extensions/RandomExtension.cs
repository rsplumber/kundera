namespace Core.Extensions;

public static class RandomExtension
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string RandomCharsAndNumbers(this Random random, int length)
    {
        return new(Enumerable.Repeat(Chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }
}