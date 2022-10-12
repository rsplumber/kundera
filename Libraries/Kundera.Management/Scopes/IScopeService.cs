namespace Kundera.Management.Scopes;

public interface IScopeService
{
    ValueTask CreateAsync(string name, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(string id, CancellationToken cancellationToken = default);

    ValueTask ChangeStatusAsync(string id, ScopeStatus status = ScopeStatus.Active, CancellationToken cancellationToken = default);

    ValueTask AddServicesAsync(string id, string[] serviceIds, CancellationToken cancellationToken = default);

    ValueTask RemoveServicesAsync(string id, string[] serviceIds, CancellationToken cancellationToken = default);

    ValueTask AddRolesAsync(string id, string[] roleIds, CancellationToken cancellationToken = default);

    ValueTask RemoveRolesAsync(string id, string[] roleIds, CancellationToken cancellationToken = default);

    ValueTask<ScopeResponse> GetAsync(string id, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<ScopesResponse>> GetAsync(CancellationToken cancellationToken = default);
}