using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Mess.MeasurementDevice.Parsers.Egauge;
using Mess.Xunit.Extensions.Microsoft;
using Mess.EventStore.Test.Abstractions;
using Mess.Timeseries.Test.Abstractions;
using Mess.MeasurementDevice.Abstractions.Parsers;

namespace Mess.MeasurementDevice.Test;

public class Startup
{
  public IHostBuilder CreateHostBuilder()
  {
    return Host.CreateDefaultBuilder();
  }

  public void ConfigureHost(IHostBuilder hostBuilder)
  {
    hostBuilder.ConfigureLogging(builder => builder.ClearProviders());
  }

  public void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    services.RegisterTestTenants();
    services.RegisterTestEventStore();
    services.RegisterTimeseriesTestMigrator();

    services.AddSingleton<IMeasurementParser, EgaugeParser>();
  }

  public void Configure(IServiceProvider services)
  {
    // NOTE: this one doesn't respect IConfiguration
    services
      .GetRequiredService<ILoggerFactory>()
      .AddProvider(
        new XunitTestOutputLoggerProvider(
          services.GetRequiredService<ITestOutputHelperAccessor>(),
          (_, level) => level is >= LogLevel.Debug and < LogLevel.None
        )
      );
  }
}
