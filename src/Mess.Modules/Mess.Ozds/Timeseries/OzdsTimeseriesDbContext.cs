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

  public DbSet<AbbEnergyBoundsQuarterHourlyEntity> AbbEnergyBoundsQuarterHourlylies { get; set; } =
    default!;

  public DbSet<AbbEnergyBoundsMonthlyEntity> AbbEnergyBoundsMonthlies { get; set; } =
    default!;

  public DbSet<SchneiderEnergyBoundsQuarterHourlyEntity> SchneiderEnergyBoundsQuarterHourlylies { get; set; } =
    default!;

  public DbSet<SchneiderEnergyBoundsMonthlyEntity> SchneiderEnergyBoundsMonthlies { get; set; } =
    default!;
}
