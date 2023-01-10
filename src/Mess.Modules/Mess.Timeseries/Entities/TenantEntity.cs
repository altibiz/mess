using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Entities;

public abstract class TenantEntity
{
  [Key]
  public string Id { get; set; } = default!;

  [Column(TypeName = "varchar")]
  public string Tenant { get; set; } = default!;
}
