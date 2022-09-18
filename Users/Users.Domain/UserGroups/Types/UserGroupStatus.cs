using Tes.Domain.Contracts;
using Users.Domain.Users.Exception;

namespace Users.Domain.UserGroups.Types;

public class UserGroupStatus : CustomType<string, UserGroupStatus>
{
    public static readonly UserGroupStatus Enable = From(nameof(Enable));
    public static readonly UserGroupStatus Disable = From(nameof(Disable));

    protected override void Validate()
    {
        if (Value is not (
            nameof(Enable)
            or nameof(Disable))
           )
        {
            throw new UserStatusNotSupportedException(Value);
        }

        base.Validate();
    }
}