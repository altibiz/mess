using Npgsql;

namespace Mess.Timeseries.Abstractions.Client;

public static class TimeseriesConnectionServiceProviderExtensions
{
  public static async Task<T> WithTimeseriesConnectionAsync<T>(
    this IServiceProvider services,
    Func<ITimeseriesConnection, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<ITimeseriesConnection>();
    await connection.Value.OpenAsync();
    var result = await todo(connection);
    await connection.Value.CloseAsync();
    return result;
  }

  public static T WithTimeseriesConnection<T>(
    this IServiceProvider services,
    Func<ITimeseriesConnection, T> todo
  )
  {
    using var scope = services.CreateScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<ITimeseriesConnection>();
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
      scope.ServiceProvider.GetRequiredService<ITimeseriesConnection>();
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
      scope.ServiceProvider.GetRequiredService<ITimeseriesConnection>();
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
      scope.ServiceProvider.GetRequiredService<ITimeseriesConnection>();
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
      scope.ServiceProvider.GetRequiredService<ITimeseriesConnection>();
    connection.Value.Open();
    using var instance = new NpgsqlCommand(command, connection.Value);
    var result = todo(instance);
    connection.Value.Close();
    return result;
  }
}
