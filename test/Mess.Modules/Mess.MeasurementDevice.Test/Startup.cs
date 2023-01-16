using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Mess.MeasurementDevice.Abstractions.Parsers.Egauge;
using Mess.MeasurementDevice.Parsers.Egauge;

// NOTE: leaving this here if someone else tries to test with mocks
// using Mess.EventStore.Test.Extensions.Moq;
// using Marten;

// TODO: try testcontainers when docker-compose and timescaledb are supported

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
    services.AddSingleton<IEgaugeParser, EgaugeParser>();
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
