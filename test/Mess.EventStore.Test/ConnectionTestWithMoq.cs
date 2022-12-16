using Xunit;
using Mess.EventStore.Client;
using Mess.EventStore.Test.Extensions.Moq;
using Marten.Linq;
using Moq;

// NOTE: leaving this here if someone else tries to test with mocks

namespace Mess.EventStore.Test;

public class ConnectionTestWithMoq
{
  [Fact(Skip = "Mocking Marten doesn't work")]
  public async Task ConnectionCheckTestAsync()
  {
    var queryCalled = false;
    var firstOrDefaultCalled = false;

    Services.SetupQuery(query =>
    {
      query
        .Setup(query => query.Query<EventStoreConnectionCheckDocumentType>())
        .Returns(() =>
        {
          var queryable =
            new Mock<IMartenQueryable<EventStoreConnectionCheckDocumentType>>();
          queryable
            // FIX: this is the part that fails to mock because FirstOrDefault
            // is an extension method
            // NOTE: currently, the only way to fake extensions/statics is to
            // use Microsoft Fakes which is sealed behind the giant paywall of
            // Microsoft Visual Studio Enterprise
            .Setup(queryable => queryable.FirstOrDefault())
            .Callback(() => firstOrDefaultCalled = true);
          return queryable.Object;
        })
        .Callback(() => queryCalled = true);
    });

    Assert.True(await Client.CheckConnectionAsync());
    Assert.True(queryCalled);
    Assert.True(firstOrDefaultCalled);
  }

  [Fact(Skip = "Mocking Marten doesn't work")]
  public void ConnectionCheckTest()
  {
    var queryCalled = false;
    var firstOrDefaultCalled = false;

    Services.SetupQuery(query =>
    {
      query
        .Setup(query => query.Query<EventStoreConnectionCheckDocumentType>())
        .Returns(() =>
        {
          var queryable =
            new Mock<IMartenQueryable<EventStoreConnectionCheckDocumentType>>();
          queryable
            // FIX: this is the part that fails to mock because FirstOrDefault
            // is an extension method
            // NOTE: currently, the only way to fake extensions/statics is to
            // use Microsoft Fakes which is sealed behind the giant paywall of
            // Microsoft Visual Studio Enterprise
            .Setup(queryable => queryable.FirstOrDefault())
            .Callback(() => firstOrDefaultCalled = true);
          return queryable.Object;
        })
        .Callback(() => queryCalled = true);
    });

    Assert.True(Client.CheckConnection());
    Assert.True(queryCalled);
    Assert.True(firstOrDefaultCalled);
  }

  public ConnectionTestWithMoq(
    IEventStoreClient client,
    IServiceProvider services
  )
  {
    Client = client;
    Services = services;
  }

  private IEventStoreClient Client { get; }
  private IServiceProvider Services { get; }
}
