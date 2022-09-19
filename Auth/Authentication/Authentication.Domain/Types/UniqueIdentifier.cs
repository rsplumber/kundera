using Tes.Domain.Contracts;

namespace Authentication.Domain.Types;

public class UniqueIdentifier : CustomType<string, UniqueIdentifier>, IIdentity
{
    public static implicit operator string(UniqueIdentifier uniqueIdentifier) => uniqueIdentifier.Value;

    public static implicit operator UniqueIdentifier(string uniqueIdentifier) => From(uniqueIdentifier);
}