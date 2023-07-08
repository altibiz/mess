using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.Client;
using Marten;
using Marten.Events.Projections;
using Mess.EventStore.Projections;
using Mess.EventStore.Abstractions.Session;
using Mess.EventStore.Session;
using Mess.OrchardCore.Extensions.OrchardCore;
using OrchardCore.Environment.Shell;
using Mess.System.Extensions.Microsoft;
using Mess.EventStore.Abstractions.Events;
using JasperFx.CodeGeneration;
using Microsoft.Extensions.Hosting;
using Marten.Services.Json.Transformations;
using Microsoft.Extensions.Logging;
using Weasel.Core;

// using Mess.EventStore.Extensions.Marten;

namespace Mess.EventStore;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services
      .AddMarten(
        (IServiceProvider services) =>
        {
          var hostEnvironment = services.GetRequiredService<IHostEnvironment>();
          var shellSettings = services.GetRequiredService<ShellSettings>();
          var databaseTablePrefix = shellSettings.GetDatabaseTablePrefix();
          var databaseConnectionString =
            shellSettings.GetDatabaseConnectionString();

          var options = new StoreOptions();

          options.AutoCreateSchemaObjects = hostEnvironment.IsDevelopment()
            ? AutoCreate.All
            : AutoCreate.CreateOrUpdate;

          options.MultiTenantedDatabases(databases =>
          {
            databases
              .AddMultipleTenantDatabase(databaseConnectionString)
              .ForTenants(new[] { databaseTablePrefix });
          });

          var eventTypes = AppDomain.CurrentDomain
            .FindTypesAssignableTo<IEvent>()
            .Where(type => !type.IsAbstract)
            .ToArray();
          var upcasters = AppDomain.CurrentDomain
            .FindTypesAssignableTo<IEventUpcaster>()
            .Where(type => !type.IsAbstract)
            .Select(type =>
            {
              try
              {
                return Activator.CreateInstance(type, new[] { services });
              }
              catch (Exception exception)
              {
                var logger = (
                  services.GetRequiredService(
                    typeof(ILogger<>).MakeGenericType(type)
                  ) as ILogger
                )!;
                logger.LogError(exception, "Error activating");
                return null;
              }
            })
            .Cast<IEventUpcaster>()
            .Where(upcaster => upcaster != null)
            .ToArray();
          options.Events.AddEventTypes(eventTypes);
          options.Events.Upcast(upcasters);

          var projection = new Projection(services);
          options.Projections.Add(
            projection,
            // ProjectionLifecycle.Async
            ProjectionLifecycle.Inline
          );

          return options;
        }
      )
      // .AddOrchardCoreAsyncProjectionDaemon()
      .OptimizeArtifactWorkflow(
        _hostEnvironment.IsDevelopment()
          ? TypeLoadMode.Auto
          : TypeLoadMode.Static
      );

    services.AddScoped<IEventStoreSession, EventStoreSession>();
    services.AddScoped<IEventStoreQuery, EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();
  }

  public Startup(IHostEnvironment hostEnvironment)
  {
    _hostEnvironment = hostEnvironment;
  }

  private readonly IHostEnvironment _hostEnvironment;
}
