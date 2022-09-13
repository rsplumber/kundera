namespace RoleManagements.Domain.UserRoles.Events;

public record UserRoleCreatedEvent(UserId UserId) : DomainEvent;