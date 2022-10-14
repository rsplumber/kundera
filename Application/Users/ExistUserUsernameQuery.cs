using Domain.Users;
using Kite.CQRS.Contracts;

namespace Application.Users;

public sealed record ExistUserUsernameQuery(Username Username) : Query<bool>;