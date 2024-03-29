using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Abstractions.Entities;

public abstract class QuarterHourlyContinuousAggregateEntity : ContinuousAggregateEntity
{
  [NotMapped] private TimeSpan? _timeSpan;

  [NotMapped]
  public TimeSpan TimeSpan => _timeSpan ??= Timestamp.AddMinutes(15) - Timestamp;
}
