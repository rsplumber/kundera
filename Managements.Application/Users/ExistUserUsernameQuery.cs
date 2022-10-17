using Kite.CQRS.Contracts;
using Managements.Domain.Users;

namespace Managements.Application.Users;

public sealed record ExistUserUsernameQuery(Username Username) : Query<bool>;