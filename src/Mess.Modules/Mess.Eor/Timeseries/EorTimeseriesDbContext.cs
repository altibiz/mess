using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Eor.Iot;

public class EorTimeseriesDbContext : TimeseriesDbContext
{
  public EorTimeseriesDbContext(
    DbContextOptions<EorTimeseriesDbContext> options,
    IServiceProvider services
  )
    : base(options, services)
  {
  }

  public DbSet<EorMeasurementEntity> EorMeasurements { get; set; } = default!;

  public DbSet<EorStatusEntity> EorStatuses { get; set; } = default!;
}
