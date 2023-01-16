namespace Mess.Tenants;

public interface ITenants
{
  public Tenant Current { get; }

  public IReadOnlyList<Tenant> All { get; }

  public void Impersonate(Tenant tenant, Action action);

  public Task ImpersonateAsync(Tenant tenant, Func<Task> action);
}
