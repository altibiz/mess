using Marten;
using Mess.System.Test.Extensions.Xunit;
using Xunit.DependencyInjection;

namespace Mess.EventStore.Test.Abstractions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddTestEventStore(
    this IServiceCollection services
  )
  {
    services.AddScoped<IDocumentStore>(services =>
    {
      var testId = services
        .GetRequiredService<ITestOutputHelperAccessor>()
        .Output.GetTest()
        .GetTestIdentifier();

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
