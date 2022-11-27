namespace Core.Hashing;

public interface IHashService
{
    string Hash(params string[] values);
}