using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Marten;
using Mess.Tenants;
using Mess.Xunit.Tenants;
using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.Client;
using Mess.EventStore.Abstractions.Parsers.Egauge;
using Mess.EventStore.Parsers.Egauge;

// NOTE: leaving this here if someone else tries to test with mocks
// using Mess.EventStore.Test.Extensions.Moq;
// using Marten;

// TODO: try testcontainers when docker-compose and timescaledb are supported

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

    services.AddScoped<ITenantProvider, TestTenantProvider>();

    // NOTE: leaving this here if someone else tries to test with mocks
    // services.AddMock<IDocumentStore>();

    // NOTE: this is not intended to be scoped but we register it as scoped
    // because we want a different IDocumentStore for each test
    services.AddScoped<IDocumentStore>(services =>
    {
      var tenant = services.GetRequiredService<ITenantProvider>();
      return DocumentStore.For(options =>
      {
        options.MultiTenantedDatabases(databases =>
        {
          databases.AddSingleTenantDatabase(
            tenant.GetTenantConnectionString(),
            tenant.GetTenantName()
          );
        });
      });
    });
    services.AddScoped<IDocumentSession>(
      services => services.GetRequiredService<IDocumentStore>().OpenSession()
    );
    services.AddScoped<IQuerySession>(
      services => services.GetRequiredService<IDocumentStore>().QuerySession()
    );

    services.AddScoped<IEventStoreSession, EventStoreSession>();
    services.AddScoped<IEventStoreQuery, EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();

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
