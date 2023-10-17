using Microsoft.EntityFrameworkCore;
using Mess.Timeseries.Abstractions.Context;
using OrchardCore.Environment.Shell;

namespace Mess.Iot.Timeseries;

public class IotTimeseriesDbContext : TimeseriesDbContext
{
  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;

  public IotTimeseriesDbContext(
    DbContextOptions<IotTimeseriesDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings) { }
}
