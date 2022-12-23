using Xunit;
using Mess.EventStore.Client;
using Mess.Util.OrchardCore.Tenants;

namespace Mess.EventStore.Test;

public record class ConnectionTest(
  IEventStoreClient client,
  ITenantProvider tenant
) : DatabaseIsolatedTest(client, tenant)
{
  [Fact]
  public async Task ConnectionCheckTestAsync()
  {
    Assert.True(await Client.CheckConnectionAsync());
  }

  [Fact]
  public void ConnectionCheckTest()
  {
    Assert.True(Client.CheckConnection());
  }
}
