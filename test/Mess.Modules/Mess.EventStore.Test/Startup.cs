﻿using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Marten;
using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.Client;

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

    services.AddScoped<ITenants, TestTenants>();

    // NOTE: leaving this here if someone else tries to test with mocks
    // services.AddMock<IDocumentStore>();

    // NOTE: this is not intended to be scoped but we register it as scoped
    // because we want a different IDocumentStore for each test
    services.AddScoped<IDocumentStore>(services =>
    {
      var tenant = services.GetRequiredService<ITenants>();
      return DocumentStore.For(options =>
      {
        options.MultiTenantedDatabases(databases =>
        {
          databases.AddSingleTenantDatabase(
            tenant.Current.ConnectionString,
            tenant.Current.Name
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
