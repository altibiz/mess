using Mess.Timeseries.Abstractions.Connection;
using Npgsql;

namespace Mess.Timeseries.Abstractions.Extensions;

public static class ITimeseriesDbConnectionIServiceProviderExtensions
{
  public static async Task<T> WithTimeseriesDbConnectionAsync<T>(
    this IServiceProvider services,
    Func<ITimeseriesDbConnection, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<ITimeseriesDbConnection>();
    await connection.Value.OpenAsync();
    var result = await todo(connection);
    await connection.Value.CloseAsync();
    return result;
  }

  public static T WithTimeseriesDbConnection<T>(
    this IServiceProvider services,
    Func<ITimeseriesDbConnection, T> todo
  )
  {
    using var scope = services.CreateScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<ITimeseriesDbConnection>();
    connection.Value.Open();
    var result = todo(connection);
    connection.Value.Close();
    return result;
  }

  public static async Task<T> WithTimeseriesDbTransactionAsync<T>(
    this IServiceProvider services,
    Func<NpgsqlTransaction, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<ITimeseriesDbConnection>();
    await connection.Value.OpenAsync();
    await using var transaction =
      await connection.Value.BeginTransactionAsync();
    var result = await todo(transaction);
    await transaction.CommitAsync();
    await connection.Value.CloseAsync();
    return result;
  }

  public static T WithTimeseriesDbTransaction<T>(
    this IServiceProvider services,
    Func<NpgsqlTransaction, T> todo
  )
  {
    using var scope = services.CreateScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<ITimeseriesDbConnection>();
    connection.Value.Open();
    using var transaction = connection.Value.BeginTransaction();
    var result = todo(transaction);
    transaction.Commit();
    connection.Value.Close();
    return result;
  }

  public static async Task<T> WithTimeseriesDbCommandAsync<T>(
    this IServiceProvider services,
    string command,
    Func<NpgsqlCommand, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<ITimeseriesDbConnection>();
    await connection.Value.OpenAsync();
    await using var instance = new NpgsqlCommand(command, connection.Value);
    var result = await todo(instance);
    await connection.Value.CloseAsync();
    return result;
  }

  public static T WithTimeseriesDbCommand<T>(
    this IServiceProvider services,
    string command,
    Func<NpgsqlCommand, T> todo
  )
  {
    using var scope = services.CreateScope();
    var connection =
      scope.ServiceProvider.GetRequiredService<ITimeseriesDbConnection>();
    connection.Value.Open();
    using var instance = new NpgsqlCommand(command, connection.Value);
    var result = todo(instance);
    connection.Value.Close();
    return result;
  }
}
