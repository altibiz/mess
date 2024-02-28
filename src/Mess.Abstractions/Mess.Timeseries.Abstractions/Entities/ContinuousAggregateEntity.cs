using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Abstractions.Entities;

public abstract class ContinuousAggregateEntity
{
  [Column(TypeName = "int8")]
  public int AggregateCount { get; set; } = default!;

  [NotMapped] private DateTimeOffset _timestamp;

  [Column(TypeName = "text")] public string Tenant { get; set; } = default!;

  [Column(TypeName = "text")] public string Source { get; set; } = default!;

  [Column(TypeName = "timestamptz")]
  public DateTimeOffset Timestamp
  {
    get => _timestamp.ToUniversalTime();
    set => _timestamp = value.ToUniversalTime();
  }
}
