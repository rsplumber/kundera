using Tes.Domain.Contracts;

namespace Users.Domain;

public class User : AggregateRoot<UserId>
{
    public User(UserId id) : base(UserId.Generate())
    {
    }
}