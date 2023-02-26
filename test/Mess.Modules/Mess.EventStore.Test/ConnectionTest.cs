using Mess.EventStore.Abstractions.Client;

namespace Mess.EventStore.Test;

public record class ConnectionTest(
  ITenantFixture TenantFixture,
  IEventStoreClient EventStoreClient
)
{
  [Fact]
  public async Task ConnectionCheckTestAsync()
  {
    Assert.True(await EventStoreClient.CheckConnectionAsync());
  }

  [Fact]
  public void ConnectionCheckTest()
  {
    Assert.True(EventStoreClient.CheckConnection());
  }
}
