namespace Core.Hashing;

public interface IHashService
{
    Task<string> HashAsync(string key, params string[] values);
}