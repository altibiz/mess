using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Mess.Util.OrchardCore.Tenants;
using Mess.Timeseries.Abstractions.Client;
using Mess.Timeseries.Client;

namespace Mess.Timeseries.Test;

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
    services.AddScoped<ITenantProvider, TestTenantProvider>();

    services.AddScoped<TimeseriesConnection>();
    services.AddSingleton<ITimeseriesClient, TimeseriesClient>();
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
