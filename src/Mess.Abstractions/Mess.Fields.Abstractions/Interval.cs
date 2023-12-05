namespace Mess.Fields.Abstractions;

public record Interval(IntervalUnit Unit, uint Count);

public enum IntervalUnit
{
  Millisecond,
  Second,
  Minute,
  Hour,
  Day,
  Week,
  Month,
  Year
}

public static class IntervalExtensions
{
  public static TimeSpan ToTimeSpan(this Interval interval)
  {
    return interval.Unit switch
    {
      IntervalUnit.Millisecond => TimeSpan.FromMilliseconds(interval.Count),
      IntervalUnit.Second => TimeSpan.FromSeconds(interval.Count),
      IntervalUnit.Minute => TimeSpan.FromMinutes(interval.Count),
      IntervalUnit.Hour => TimeSpan.FromHours(interval.Count),
      IntervalUnit.Day => TimeSpan.FromDays(interval.Count),
      IntervalUnit.Week => TimeSpan.FromDays(interval.Count * 7),
      IntervalUnit.Month => TimeSpan.FromDays(interval.Count * 30),
      IntervalUnit.Year => TimeSpan.FromDays(interval.Count * 365),
      _
        => throw new ArgumentOutOfRangeException(
          nameof(interval),
          interval,
          null
        )
    };
  }

  public static Interval ToInterval(this TimeSpan timeSpan)
  {
    return timeSpan < TimeSpan.FromMilliseconds(1)
      ? new Interval(IntervalUnit.Millisecond, 0)
      : timeSpan < TimeSpan.FromSeconds(1)
        ? new Interval(
          IntervalUnit.Millisecond,
          (uint)timeSpan.TotalMilliseconds
        )
        : timeSpan < TimeSpan.FromMinutes(1)
          ? new Interval(IntervalUnit.Second, (uint)timeSpan.TotalSeconds)
          : timeSpan < TimeSpan.FromHours(1)
            ? new Interval(IntervalUnit.Minute, (uint)timeSpan.TotalMinutes)
            : timeSpan < TimeSpan.FromDays(1)
              ? new Interval(IntervalUnit.Hour, (uint)timeSpan.TotalHours)
              : timeSpan < TimeSpan.FromDays(7)
                ? new Interval(IntervalUnit.Day, (uint)timeSpan.TotalDays)
                : timeSpan < TimeSpan.FromDays(30)
                  ? new Interval(
                    IntervalUnit.Week,
                    (uint)(timeSpan.TotalDays / 7)
                  )
                  : timeSpan < TimeSpan.FromDays(365)
                    ? new Interval(
                      IntervalUnit.Month,
                      (uint)(timeSpan.TotalDays / 30)
                    )
                    : new Interval(
                      IntervalUnit.Year,
                      (uint)(timeSpan.TotalDays / 365)
                    );
  }

  public static string ToHumanString(this IntervalUnit intervalUnit)
  {
    return intervalUnit switch
    {
      IntervalUnit.Millisecond => "milliseconds",
      IntervalUnit.Second => "seconds",
      IntervalUnit.Minute => "minutes",
      IntervalUnit.Hour => "hours",
      IntervalUnit.Day => "days",
      IntervalUnit.Week => "weeks",
      IntervalUnit.Month => "months",
      IntervalUnit.Year => "years",
      _
        => throw new ArgumentOutOfRangeException(
          nameof(intervalUnit),
          intervalUnit,
          null
        )
    };
  }
}
