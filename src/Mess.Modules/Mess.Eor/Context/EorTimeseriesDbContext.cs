using Mess.Eor.Entities;
using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Eor.Context;

public class EorTimeseriesDbContext : TimeseriesDbContext
{
  public DbSet<EorMeasurementEntity> EorMeasurements { get; set; } = default!;

  public DbSet<EorStatusEntity> EorStatuses { get; set; } = default!;

  public EorTimeseriesDbContext(
    DbContextOptions<EorTimeseriesDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings) { }
}
