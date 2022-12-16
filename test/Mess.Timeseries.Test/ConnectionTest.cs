using Xunit;
using Mess.Timeseries.Client;
using Mess.Util.OrchardCore.Tenants;

namespace Mess.Timeseries.Test;

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

  public ConnectionTest(ITimeseriesClient client, ITenantProvider tenant)
    : base(client, tenant) { }
}
