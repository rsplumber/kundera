using Tes.Domain.Contracts;

namespace Users.Domain;

public class Name : CustomType<string, Name>
{
    public static implicit operator string(Name name) => name.Value;

    public static implicit operator Name(string name) => From(name);
}