using Xunit;
using Mess.EventStore.Client;
using Mess.Util.OrchardCore.Tenants;

namespace Mess.EventStore.Test;

public class ConnectionTest : DatabaseIsolatedTest
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

  public ConnectionTest(IEventStoreClient client, ITenantProvider tenant)
    : base(client, tenant) { }
}
