using Xunit;
using Mess.Timeseries.Abstractions.Client;
using Mess.Tenants;
using Mess.Xunit.Tenants;

namespace Mess.Timeseries.Test;

public record class ConnectionTest(
  ITimeseriesClient client,
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
