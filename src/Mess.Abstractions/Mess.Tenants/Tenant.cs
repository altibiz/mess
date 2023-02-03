namespace Mess.Tenants;

public record struct Tenant(
  string Name,
  string ConnectionString,
  string TablePrefix
);
