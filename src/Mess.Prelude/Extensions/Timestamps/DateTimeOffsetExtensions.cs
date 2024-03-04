namespace Mess.Prelude.Extensions.Timestamps;

public static class DateTimeOffsetExtensions
{
  public static (DateTimeOffset, DateTimeOffset) GetMonthRange(
    this DateTimeOffset dateTimeOffset
  )
  {
    var monthStart = new DateTimeOffset(
      dateTimeOffset.Year,
      dateTimeOffset.Month,
      1,
      0,
      0,
      0,
      dateTimeOffset.Offset
    );
    var monthEnd = monthStart.AddMonths(1);
    return (monthStart, monthEnd);
  }

  public static DateTimeOffset GetStartOfQuarterHour(this DateTimeOffset dateTime)
  {
    int quarterHour = dateTime.Minute / 15;
    return new DateTimeOffset(
      dateTime.Year,
      dateTime.Month,
      dateTime.Day,
      dateTime.Hour,
      quarterHour * 15,
      0,
      dateTime.Offset
    );
  }

  public static DateTimeOffset GetStartOfMonth(this DateTimeOffset dateTime)
  {
    return new DateTimeOffset(
      dateTime.Year,
      dateTime.Month,
      1,
      0,
      0,
      0,
      dateTime.Offset
    );
  }

  public static DateTimeOffset GetStartOfDay(this DateTimeOffset dateTime)
  {
    return new DateTimeOffset(
      dateTime.Year,
      dateTime.Month,
      dateTime.Day,
      0,
      0,
      0,
      dateTime.Offset
    );
  }
}
