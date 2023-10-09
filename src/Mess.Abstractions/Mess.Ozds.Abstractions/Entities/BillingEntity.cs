using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Ozds.Abstractions.Entities;

public class BillingEntity : HypertableEntity
{
  [Column(TypeName = "bigint")]
  public long Milliseconds => Timestamp.Ticks / TimeSpan.TicksPerMillisecond;

  [Column(TypeName = "float4")]
  public float? Energy { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? LowEnergy { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? HighEnergy { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? Power { get; set; } = default!;
}
