using Xunit;
using Mess.EventStore.Abstractions.Client;
using Mess.Xunit.Tenants;

namespace Mess.EventStore.Test;

public record class ConnectionTest(
  IEventStoreClient client,
  ITenantProvider tenant
) : DatabaseIsolatedTest(tenant)
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
