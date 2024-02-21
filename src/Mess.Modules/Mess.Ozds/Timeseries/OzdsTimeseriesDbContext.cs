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

  public DbSet<AbbQuarterHourlyEnergyBoundsEntity> AbbQuarterHourlyEnergyBounds { get; set; } =
    default!;

  public DbSet<AbbMonthlyEnergyBoundsEntity> AbbMonthlyEnergyBounds { get; set; } =
    default!;

  public DbSet<SchneiderQuarterHourlyEnergyBoundsEntity> SchneiderQuarterHourlyEnergyBounds { get; set; } =
    default!;

  public DbSet<SchneiderMonthlyEnergyBoundsEntity> SchneiderMonthlyEnergyBounds { get; set; } =
    default!;
}
