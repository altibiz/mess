using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Relational.Abstractions.Entities;

public abstract class TenantedEntity
{
  [Column(TypeName = "text")] public string Tenant { get; set; } = default!;

  [Column(TypeName = "bigserial")] public int Id { get; set; }
}
