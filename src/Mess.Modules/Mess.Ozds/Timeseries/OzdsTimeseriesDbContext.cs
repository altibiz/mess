using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Ozds.Timeseries;

public class OzdsTimeseriesDbContext : TimeseriesDbContext
{
  public OzdsTimeseriesDbContext(
    DbContextOptions<OzdsTimeseriesDbContext> options,
    IServiceProvider services
  )
    : base(options, services)
  {
  }

  public DbSet<AbbMeasurementEntity> AbbMeasurements { get; set; } = default!;

  public DbSet<SchneiderMeasurementEntity> SchneiderMeasurements { get; set; } =
    default!;

  public DbSet<AbbQuarterHourlyAggregateEntity> AbbQuarterHourlyAggregate { get; set; } =
    default!;

  public DbSet<AbbDailyAggregateEntity> AbbDailyAggregate { get; set; } =
    default!;

  public DbSet<AbbMonthlyAggregateEntity> AbbMonthlyAggregate { get; set; } =
    default!;

  public DbSet<SchneiderQuarterHourlyAggregateEntity> SchneiderQuarterHourlyAggregate { get; set; } =
    default!;

  public DbSet<SchneiderDailyAggregateEntity> SchneiderDailyAggregate { get; set; } =
    default!;

  public DbSet<SchneiderMonthlyAggregateEntity> SchneiderMonthlyAggregate { get; set; } =
    default!;
}
