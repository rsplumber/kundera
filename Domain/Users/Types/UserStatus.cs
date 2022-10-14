using Domain.Users.Exception;
using Kite.CustomType;

namespace Domain.Users.Types;

public class UserStatus : CustomType<string, UserStatus>
{
    public static readonly UserStatus Active = From(nameof(Active));
    public static readonly UserStatus Suspend = From(nameof(Suspend));
    public static readonly UserStatus Block = From(nameof(Block));

    protected override void Validate()
    {
        if (Value is not (nameof(Active) or nameof(Suspend) or nameof(Block)))
        {
            throw new UserStatusNotSupportedException(Value);
        }

        base.Validate();
    }
}