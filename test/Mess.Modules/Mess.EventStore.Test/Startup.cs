using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.Client;
using Mess.Xunit.Extensions.Microsoft;
using Mess.EventStore.Test.Abstractions;

namespace Mess.EventStore.Test;

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
    var environment = hostBuilderContext.HostingEnvironment;
    var configuration = hostBuilderContext.Configuration;

    services.RegisterTestTenants();
    services.RegisterTestEventStore();

    services.AddScoped<IEventStoreSession, EventStoreSession>();
    services.AddScoped<IEventStoreQuery, EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();
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
