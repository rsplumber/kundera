using Core.Roles;
using Mediator;

namespace Application.Roles.Create;

public sealed record CreateRoleCommand : ICommand<Role>
{
    public string Name { get; init; } = default!;

    public IDictionary<string, string>? Meta { get; init; }
}

internal sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Role>
{
    private readonly IRoleFactory _roleFactory;

    public CreateRoleCommandHandler(IRoleFactory roleFactory)
    {
        _roleFactory = roleFactory;
    }

    public async ValueTask<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleFactory.CreateAsync(command.Name, command.Meta);
        return role;
    }
}