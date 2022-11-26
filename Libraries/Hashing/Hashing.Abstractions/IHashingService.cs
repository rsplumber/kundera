namespace Hashing.Abstractions;

public interface IHashService
{
    string Hash(params string[] values);
}