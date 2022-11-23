using Managements.Domain.Contracts;

namespace Managements.Domain.Groups.Exception;

public sealed class GroupNameDuplicateException : DomainException
{
    private const string DefaultMessage = "Group name is duplicate";

    public GroupNameDuplicateException() : base(DefaultMessage)
    {
    }
}