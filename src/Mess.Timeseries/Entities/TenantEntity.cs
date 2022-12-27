using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Entities;

public abstract class TenantEntity
{
  [Column(TypeName = "string")]
  public string Tenant { get; set; } = default!;
}
