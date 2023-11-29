using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.Event.Abstractions.Client;
using Mess.Event.Client;
using Marten;
using Marten.Events.Projections;
using Mess.Event.Projections;
using Mess.Event.Abstractions.Session;
using Mess.Event.Session;
using Mess.OrchardCore.Extensions.OrchardCore;
using OrchardCore.Environment.Shell;
using Mess.System.Extensions.Microsoft;
using Mess.Event.Abstractions.Services;
using JasperFx.CodeGeneration;
using Microsoft.Extensions.Hosting;
using Marten.Services.Json.Transformations;
using Microsoft.Extensions.Logging;
using Weasel.Core;

// using Mess.Event.Extensions.Marten;

namespace Mess.Event;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddMarten(
      (IServiceProvider services) =>
      {
        var hostEnvironment = services.GetRequiredService<IHostEnvironment>();
        var shellSettings = services.GetRequiredService<ShellSettings>();
        var databaseTablePrefix = shellSettings.GetDatabaseTablePrefix();
        var databaseConnectionString =
          shellSettings.GetDatabaseConnectionString();

        var options = new StoreOptions
        {
          AutoCreateSchemaObjects = hostEnvironment.IsDevelopment()
            ? AutoCreate.All
            : AutoCreate.CreateOrUpdate,
          GeneratedCodeMode = TypeLoadMode.Auto
        };

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
    );
    // .AddOrchardCoreAsyncProjectionDaemon();

    services.AddScoped<IEventStoreSession, EventStoreSession>();
    services.AddScoped<IEventStoreQuery, EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();
  }
}
