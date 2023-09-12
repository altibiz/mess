using Microsoft.EntityFrameworkCore;
using Mess.Timeseries.Abstractions.Context;
using Mess.MeasurementDevice.Entities;
using OrchardCore.Environment.Shell;

namespace Mess.MeasurementDevice.Context;

public class MeasurementDbContext : TimeseriesDbContext
{
  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;

  public DbSet<AbbMeasurementEntity> AbbMeasurements { get; set; } = default!;

  public MeasurementDbContext(
    DbContextOptions<MeasurementDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings) { }
}
