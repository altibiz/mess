using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Client;

namespace Mess.Timeseries.Entities;

public abstract class MeasurementEntity
{
  [HypertableColumn]
  [Column(TypeName = "timestamp with time zone")]
  public DateTime Timestamp { get; set; } = default!;

  [Column(TypeName = "string")]
  public string SourceId { get; set; } = default!;

  [Column(TypeName = "string")]
  public string Tenant { get; set; } = default!;
}
