using Mess.EventStore.Abstractions.Client;

namespace Mess.EventStore.Test;

public record class ConnectionTest(
  IEventStoreClient client,
  IServiceProvider services
) : DatabaseIsolatedTest(services)
{
  [Fact]
  public async Task ConnectionCheckTestAsync()
  {
    Assert.True(await client.CheckConnectionAsync());
  }

  [Fact]
  public void ConnectionCheckTest()
  {
    Assert.True(client.CheckConnection());
  }
}
