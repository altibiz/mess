using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Abstractions.Entities;

// TODO: create attributes for migrations

public abstract class MonthlyContinuousAggregateEntity : ContinuousAggregateEntity
{
  [NotMapped] private TimeSpan? _timeSpan;

  [NotMapped]
  public TimeSpan TimeSpan => _timeSpan ??=
    new DateTimeOffset(
      Timestamp.Year,
      Timestamp.Month,
      1,
      0,
      0,
      0,
      Timestamp.Offset)
      - Timestamp;
}
