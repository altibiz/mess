using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Test;

public record class ConnectionTest(
  ITenantFixture TenantFixture,
  ITimeseriesClient TimeseriesClient
)
{
  [Fact]
  public async Task ConnectionCheckTestAsync()
  {
    Assert.True(await TimeseriesClient.CheckConnectionAsync());
  }

  [Fact]
  public void ConnectionCheckTest()
  {
    Assert.True(TimeseriesClient.CheckConnection());
  }
}
