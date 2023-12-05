using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Enms.Timeseries;

public class EnmsTimeseriesDbContext : TimeseriesDbContext
{
  public EnmsTimeseriesDbContext(
    DbContextOptions<EnmsTimeseriesDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings)
  {
  }

  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;
}
