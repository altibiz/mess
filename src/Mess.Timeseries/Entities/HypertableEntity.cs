using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Client;

namespace Mess.Timeseries.Entities;

public abstract class HypertableEntity : TenantEntity
{
  [HypertableColumn]
  [Column(TypeName = "timestamp")]
  public DateTime Timestamp { get; set; } = default!;
}
