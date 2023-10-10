namespace Mess.System.Extensions.Timestamps;

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
}
