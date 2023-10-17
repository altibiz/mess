using Microsoft.EntityFrameworkCore;
using Mess.Timeseries.Abstractions.Context;
using Mess.Ozds.Timeseries;
using OrchardCore.Environment.Shell;

namespace Mess.Ozds.Timeseries;

public class OzdsTimeseriesDbContext : TimeseriesDbContext
{
  public DbSet<AbbMeasurementEntity> AbbMeasurements { get; set; } = default!;

  public OzdsTimeseriesDbContext(
    DbContextOptions<OzdsTimeseriesDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings) { }
}
