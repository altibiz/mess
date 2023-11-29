using Npgsql;
using Mess.Timeseries.Abstractions.Connection;
using OrchardCore.Environment.Shell.Scope;

namespace Mess.Timeseries;

public sealed class TimeseriesDbConnection
  : ITimeseriesDbConnection,
    IDisposable,
    IAsyncDisposable
{
  public NpgsqlConnection Value => _value
      ?? throw new InvalidOperationException(
        "Connection has already been disposed"
      );

  public TimeseriesDbConnection()
  {
    _value = new NpgsqlConnection(
      ShellScope.Current.ShellContext.Settings.ShellConfiguration[
        "ConnectionString"
      ]
    );
  }

  private NpgsqlConnection? _value;

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
