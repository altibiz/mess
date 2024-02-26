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

  public DbSet<AbbQuarterHourlyEnergyRangeEntity> AbbQuarterHourlyEnergyRange { get; set; } =
    default!;

  public DbSet<AbbDailyEnergyRangeEntity> AbbDailyEnergyRange { get; set; } =
    default!;

  public DbSet<AbbMonthlyEnergyRangeEntity> AbbMonthlyEnergyRange { get; set; } =
    default!;

  public DbSet<SchneiderQuarterHourlyEnergyRangeEntity> SchneiderQuarterHourlyEnergyRange { get; set; } =
    default!;

  public DbSet<SchneiderDailyEnergyRangeEntity> SchneiderDailyEnergyRange { get; set; } =
    default!;

  public DbSet<SchneiderMonthlyEnergyRangeEntity> SchneiderMonthlyEnergyRange { get; set; } =
    default!;
}
