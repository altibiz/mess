using Microsoft.Extensions.DependencyInjection;
using Mess.Util.OrchardCore.Tenants;
using Npgsql;

namespace Mess.Timeseries.Client;

public sealed class TimeseriesConnection : IDisposable, IAsyncDisposable
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

internal static class ConnectionServiceProviderExtensions
{
  public static async Task<T> WithTimeseriesConnectionAsync<T>(
    this IServiceProvider services,
    Func<TimeseriesConnection, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<TimeseriesConnection>();
    await connection.Value.OpenAsync();
    var result = await todo(connection);
    await connection.Value.CloseAsync();
    return result;
  }

  public static T WithTimeseriesConnection<T>(
    this IServiceProvider services,
    Func<TimeseriesConnection, T> todo
  )
  {
    using var scope = services.CreateScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<TimeseriesConnection>();
    connection.Value.Open();
    var result = todo(connection);
    connection.Value.Close();
    return result;
  }

  public static async Task<T> WithTimeseriesTransactionAsync<T>(
    this IServiceProvider services,
    Func<NpgsqlTransaction, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<TimeseriesConnection>();
    await connection.Value.OpenAsync();
    await using var transaction =
      await connection.Value.BeginTransactionAsync();
    var result = await todo(transaction);
    await transaction.CommitAsync();
    await connection.Value.CloseAsync();
    return result;
  }

  public static T WithTimeseriesTransaction<T>(
    this IServiceProvider services,
    Func<NpgsqlTransaction, T> todo
  )
  {
    using var scope = services.CreateScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<TimeseriesConnection>();
    connection.Value.Open();
    using var transaction = connection.Value.BeginTransaction();
    var result = todo(transaction);
    transaction.Commit();
    connection.Value.Close();
    return result;
  }

  public static async Task<T> WithTimeseriesCommandAsync<T>(
    this IServiceProvider services,
    string command,
    Func<NpgsqlCommand, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<TimeseriesConnection>();
    await connection.Value.OpenAsync();
    await using var instance = new NpgsqlCommand(command, connection.Value);
    var result = await todo(instance);
    await connection.Value.CloseAsync();
    return result;
  }

  public static T WithTimeseriesCommand<T>(
    this IServiceProvider services,
    string command,
    Func<NpgsqlCommand, T> todo
  )
  {
    using var scope = services.CreateScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<TimeseriesConnection>();
    connection.Value.Open();
    using var instance = new NpgsqlCommand(command, connection.Value);
    var result = todo(instance);
    connection.Value.Close();
    return result;
  }
}
