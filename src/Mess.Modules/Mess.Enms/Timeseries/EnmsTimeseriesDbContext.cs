using Microsoft.EntityFrameworkCore;
using Mess.Timeseries.Abstractions.Context;
using OrchardCore.Environment.Shell;

namespace Mess.Enms.Timeseries;

public class EnmsTimeseriesDbContext : TimeseriesDbContext
{
  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;

  public EnmsTimeseriesDbContext(
    DbContextOptions<EnmsTimeseriesDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings) { }
}
