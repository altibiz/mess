using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Enms.Timeseries;

public class EnmsTimeseriesDbContext : TimeseriesDbContext
{
  public EnmsTimeseriesDbContext(
    DbContextOptions<EnmsTimeseriesDbContext> options,
    IServiceProvider services
  )
    : base(options, services)
  {
  }

  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;
}
