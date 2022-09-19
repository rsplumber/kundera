using Tes.Domain.Contracts;

namespace Authentication.Domain.Types;

public class UserId : CustomType<Guid, UserId>
{
    public static implicit operator Guid(UserId userId) => userId.Value;

    public static implicit operator UserId(Guid userId) => From(userId);
}