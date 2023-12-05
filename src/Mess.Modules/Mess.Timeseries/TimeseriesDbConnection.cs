using Mess.Timeseries.Abstractions.Connection;
using Npgsql;
using OrchardCore.Environment.Shell.Scope;

namespace Mess.Timeseries;

public sealed class TimeseriesDbConnection
  : ITimeseriesDbConnection,
    IDisposable,
    IAsyncDisposable
{
  private NpgsqlConnection? _value;

  public TimeseriesDbConnection()
  {
    _value = new NpgsqlConnection(
      ShellScope.Current.ShellContext.Settings.ShellConfiguration[
        "ConnectionString"
      ]
    );
  }

  public async ValueTask DisposeAsync()
  {
    if (_value is not null)
    {
      await _value.DisposeAsync();
      _value = null;
    }
  }

  public void Dispose()
  {
    if (_value is not null)
    {
      _value.Dispose();
      _value = null;
    }
  }

  public NpgsqlConnection Value => _value
                                   ?? throw new InvalidOperationException(
                                     "Connection has already been disposed"
                                   );
}
