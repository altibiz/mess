using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Mess.Tenants;

namespace Mess.Timeseries.Abstractions.Client;

public abstract class TimeseriesDbContextDesignTimeFactory<T>
  : IDesignTimeDbContextFactory<T>
  where T : TimeseriesDbContext
{
  public T CreateDbContext(string[] args)
  {
    var constructor = typeof(T)
      .GetConstructors()
      .FirstOrDefault(constructor =>
      {
        var parameters = constructor.GetParameters();
        return parameters.Length == 2
          && parameters[0].ParameterType == typeof(DbContextOptions<T>)
          && parameters[1].ParameterType == typeof(ITenants);
      });
    if (constructor is null)
    {
      throw new InvalidOperationException(
        $"Cannot find a suitable constructor for {typeof(T).Name}"
      );
    }

    var optionsBuilder = new DbContextOptionsBuilder<T>();

    var timeseriesContext =
      constructor.Invoke(
        new object[]
        {
          optionsBuilder.Options,
          new DesignTimeTenants(
            TenantName: "Default",
            ConnactionString: "Server=localhost;Port=5432;User Id=mess;Password=mess;Database=mess",
            TablePrefix: "default"
          )
        }
      ) as T;
    if (timeseriesContext is null)
    {
      throw new InvalidOperationException(
        $"Failed constructing {typeof(T).Name}"
      );
    }

    return timeseriesContext;
  }
}
