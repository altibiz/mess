using Marten;
using Mess.System.Test.Extensions.Microsoft;

namespace Mess.Event.Test.Abstractions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddTestEventStore(
    this IServiceCollection services
  )
  {
    services.AddScoped<IDocumentStore>(services =>
    {
      var testId = services.GetTestId();

      return DocumentStore.For(
        (options) =>
        {
          options.MultiTenantedDatabases(databases =>
          {
            databases.AddSingleTenantDatabase(
              "Server=localhost;Port=5432;User Id=mess;Password=mess;Database=mess",
              $"{testId}-test"
            );
          });
        }
      );
    });
    services.AddScoped<IDocumentSession>(
      services =>
        services.GetRequiredService<IDocumentStore>().LightweightSession()
    );
    services.AddScoped<IQuerySession>(
      services => services.GetRequiredService<IDocumentStore>().QuerySession()
    );
    return services;
  }
}
