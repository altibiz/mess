using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Abstractions.Entities;

public abstract class MonthlyContinuousAggregateEntity : ContinuousAggregateEntity
{
  [NotMapped] private TimeSpan? _timeSpan;

  [NotMapped]
  public TimeSpan TimeSpan => _timeSpan ??= Timestamp.AddMonths(1) - Timestamp;
}
