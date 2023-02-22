namespace Core.Domains.Users;

public sealed class UserStatus : Enumeration
{
    public static readonly UserStatus Active = new(1, nameof(Active));
    public static readonly UserStatus Suspend = new(2, nameof(Suspend));
    public static readonly UserStatus Block = new(3, nameof(Block));


    private UserStatus(int id, string name) : base(id, name)
    {
    }
}