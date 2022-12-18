﻿namespace Core.Domains.Groups.Exception;

public sealed class GroupNameDuplicateException : ApplicationException
{
    private const string DefaultMessage = "Group name is duplicate";

    public GroupNameDuplicateException() : base(DefaultMessage)
    {
    }
}