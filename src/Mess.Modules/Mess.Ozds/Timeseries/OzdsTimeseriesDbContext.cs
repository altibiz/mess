using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Ozds.Timeseries;

public class OzdsTimeseriesDbContext : TimeseriesDbContext
{
  public OzdsTimeseriesDbContext(
    DbContextOptions<OzdsTimeseriesDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings)
  {
  }

  public DbSet<AbbMeasurementEntity> AbbMeasurements { get; set; } = default!;

  public DbSet<SchneiderMeasurementEntity> SchneiderMeasurements { get; set; } =
    default!;
}
