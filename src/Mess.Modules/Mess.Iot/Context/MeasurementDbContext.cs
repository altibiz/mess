using Microsoft.EntityFrameworkCore;
using Mess.Timeseries.Abstractions.Context;
using Mess.Iot.Entities;
using OrchardCore.Environment.Shell;

namespace Mess.Iot.Context;

public class MeasurementDbContext : TimeseriesDbContext
{
  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;

  public MeasurementDbContext(
    DbContextOptions<MeasurementDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings) { }
}