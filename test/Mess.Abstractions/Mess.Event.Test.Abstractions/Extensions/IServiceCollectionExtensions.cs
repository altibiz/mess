using Marten;
using Mess.Prelude.Test.Extensions.Microsoft;

namespace Mess.Event.Test.Abstractions.Extensions;

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
        options =>
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
    services.AddScoped(
      services =>
        services.GetRequiredService<IDocumentStore>().LightweightSession()
    );
    services.AddScoped(
      services => services.GetRequiredService<IDocumentStore>().QuerySession()
    );
    return services;
  }
}
