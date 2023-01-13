using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Entities;

public abstract class MeasurementEntity
{
  [HypertableColumn]
  [Column(TypeName = "timestamp with time zone")]
  public DateTime Timestamp { get; set; } = default!;

  [Column(TypeName = "varchar")]
  public string SourceId { get; set; } = default!;

  [Column(TypeName = "varchar")]
  public string Tenant { get; set; } = default!;
}
