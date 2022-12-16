using Marten;
using Moq;
using System.Data;

// NOTE: leaving this here if someone else tries to test with mocks

namespace Mess.EventStore.Test.Extensions.Moq;

public static class IServiceCollectionExtensions
{
  public static void AddMock<T>(this IServiceCollection services)
    where T : class
  {
    services.AddSingleton<Mock<T>>();
    services.AddSingleton<T>(
      services => services.GetRequiredService<Mock<T>>().Object
    );
  }

  public static void SetupSession(
    this IServiceProvider services,
    Action<Mock<IDocumentSession>> sessionAction
  ) =>
    services
      .GetRequiredService<Mock<IDocumentStore>>()
      .Setup(
        store =>
          store.OpenSession(
            It.IsAny<string>(),
            It.IsAny<DocumentTracking>(),
            It.IsAny<IsolationLevel>()
          )
      )
      .Returns(() =>
      {
        var query = new Mock<IDocumentSession>();
        sessionAction(query);
        return query.Object;
      });

  public static void SetupQuery(
    this IServiceProvider services,
    Action<Mock<IQuerySession>> queryAction
  ) =>
    services
      .GetRequiredService<Mock<IDocumentStore>>()
      .Setup(store => store.QuerySession(It.IsAny<string>()))
      .Returns(() =>
      {
        var query = new Mock<IQuerySession>();
        queryAction(query);
        return query.Object;
      });
}
