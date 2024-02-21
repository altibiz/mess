using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Abstractions.Entities;

// TODO: create attributes for migrations

public abstract class ContinuousAggregateEntity
{
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
