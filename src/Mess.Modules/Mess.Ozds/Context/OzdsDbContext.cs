using Microsoft.EntityFrameworkCore;
using Mess.Timeseries.Abstractions.Context;
using Mess.Ozds.Entities;
using OrchardCore.Environment.Shell;

namespace Mess.Ozds.Context;

public class OzdsDbContext : TimeseriesDbContext
{
  public DbSet<AbbMeasurementEntity> AbbMeasurements { get; set; } = default!;

  public OzdsDbContext(
    DbContextOptions<OzdsDbContext> options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings) { }
}
