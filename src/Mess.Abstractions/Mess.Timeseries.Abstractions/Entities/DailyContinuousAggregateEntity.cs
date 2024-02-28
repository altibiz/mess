using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Abstractions.Entities;

public abstract class DailyContinuousAggregateEntity : ContinuousAggregateEntity
{
  [NotMapped] private TimeSpan? _timeSpan;

  [NotMapped]
  public TimeSpan TimeSpan => _timeSpan ??= Timestamp.AddDays(1) - Timestamp;
}
