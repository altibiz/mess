using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Mess.Timeseries.Client;

public class TimeseriesContextDesignTimeFactory
  : IDesignTimeDbContextFactory<TimeseriesContext>
{
  public TimeseriesContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<TimeseriesContext>();

    return new TimeseriesContext(
      optionsBuilder.Options,
      new DesignTimeTenantProvider(
        tenantName: "Default",
        connactionString: "Server=localhost;Port=5432;User Id=mess;Password=mess;Database=mess"
      )
    );
  }
}
