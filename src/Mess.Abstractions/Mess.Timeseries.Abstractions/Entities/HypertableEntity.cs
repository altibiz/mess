using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Abstractions.Entities;

public abstract class HypertableEntity
{
  [Column(TypeName = "varchar")]
  public string Tenant { get; set; } = default!;

  [Column(TypeName = "varchar")]
  public string Source { get; set; } = default!;

  [HypertableColumn]
  [Column(TypeName = "timestamp")]
  public DateTime Timestamp { get; set; } = default!;
}
