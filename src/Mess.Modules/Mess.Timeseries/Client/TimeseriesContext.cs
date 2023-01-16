using Microsoft.EntityFrameworkCore;
using Mess.Tenants;
using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Client;

// NOTE: no 'Db' in name because I don't want to deal with name clashing
public class TimeseriesContext : TimeseriesDbContext
{
  public TimeseriesContext(
    DbContextOptions<TimeseriesContext> options,
    ITenantProvider tenants
  ) : base(options, tenants) { }
}
