using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Eor.Iot;

public class EorTimeseriesDbContext : TimeseriesDbContext
{
  public EorTimeseriesDbContext(
    DbContextOptions<EorTimeseriesDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings)
  {
  }

  public DbSet<EorMeasurementEntity> EorMeasurements { get; set; } = default!;

  public DbSet<EorStatusEntity> EorStatuses { get; set; } = default!;
}
