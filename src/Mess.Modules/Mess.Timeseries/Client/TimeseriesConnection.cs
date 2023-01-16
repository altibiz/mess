using Npgsql;
using Mess.Tenants;
using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Client;

public sealed class TimeseriesConnection
  : ITimeseriesConnection,
    IDisposable,
    IAsyncDisposable
{
  public NpgsqlConnection Value
  {
    get =>
      _value
      ?? throw new InvalidOperationException(
        "Connection has already been disposed"
      );
  }

  public TimeseriesConnection(ITenantProvider tenant)
  {
    _value = new NpgsqlConnection(tenant.GetTenantConnectionString());
  }

  private NpgsqlConnection? _value = null;

  public void Dispose()
  {
    if (_value is not null)
    {
      _value.Dispose();
      _value = null;
    }
  }

  public async ValueTask DisposeAsync()
  {
    if (_value is not null)
    {
      await _value.DisposeAsync();
      _value = null;
    }
  }
}
