using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Hosting.Internal;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;

namespace Mess.Relational.Abstractions.Context;

public abstract class RelationalDbContextDesignTimeFactory<T>
  : IDesignTimeDbContextFactory<T>
  where T : RelationalDbContext
{
  public T CreateDbContext(string[] args)
  {
    var constructor =
      typeof(T)
        .GetConstructors()
        .FirstOrDefault(constructor =>
        {
          var parameters = constructor.GetParameters();
          return parameters.Length == 2
                 && parameters[0].ParameterType == typeof(DbContextOptions<T>)
                 && parameters[1].ParameterType == typeof(IServiceProvider);
        })
      ?? throw new InvalidOperationException(
        $"Cannot find a suitable constructor for {typeof(T).Name}"
      );

    var optionsBuilder = new DbContextOptionsBuilder<T>();
    var shellSettings = new ShellSettings(
      new ShellConfiguration(),
      new ShellConfiguration(
        new ConfigurationBuilder()
          .AddInMemoryCollection(
            new Dictionary<string, string?>
            {
              ["ConnectionString"] =
                "Server=localhost;Port=5432;User Id=mess;Password=mess;Database=mess"
            }
          )
          .Build()
      )
    )
    {
      Name = "Default"
    };

    var hostEnvironment = new HostingEnvironment()
    {
      EnvironmentName = Environments.Development
    };

    var serviceCollection = new ServiceCollection();

    serviceCollection.AddSingleton<IHostEnvironment>(hostEnvironment);
    serviceCollection.AddSingleton(shellSettings);

    var timeseriesContext =
      constructor.Invoke(
        new object[]
        {
          optionsBuilder.Options,
          serviceCollection.BuildServiceProvider()
        }
      ) as T
      ?? throw new InvalidOperationException(
        $"Failed constructing {typeof(T).Name}"
      );

    return timeseriesContext;
  }
}
