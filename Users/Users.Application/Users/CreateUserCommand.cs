using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users.Types;

namespace Users.Application.Users;

public sealed record CreateUserByUsernameCommand(Username Username, UserGroupId UserGroupId) : Command;

public sealed record CreateUserByPhoneNumberCommand(PhoneNumber PhoneNumber, UserGroupId UserGroupId) : Command;

public sealed record CreateUserByEmailCommand(Email Email, UserGroupId UserGroupId) : Command;

public sealed record CreateUserByNationalCodeCommand(NationalCode NationalCode, UserGroupId UserGroupId) : Command;