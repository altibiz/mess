using Microsoft.EntityFrameworkCore;
using Mess.Tenants;
using Mess.Timeseries.Abstractions.Client;
using Mess.Timeseries.Entities;

namespace Mess.MeasurementDevice.Client;

public class MeasurementDbContext : TimeseriesDbContext
{
  public DbSet<EgaugeMeasurementEntity> EgaugeMeasurements { get; set; } =
    default!;

  public MeasurementDbContext(
    DbContextOptions<MeasurementDbContext> options,
    ITenants tenants
  ) : base(options, tenants) { }
}
