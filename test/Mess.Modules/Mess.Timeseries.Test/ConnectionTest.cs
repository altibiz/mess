using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Test;

public record class ConnectionTest(ITimeseriesClient client, ITenants tenants)
  : DatabaseIsolatedTest(tenants)
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
