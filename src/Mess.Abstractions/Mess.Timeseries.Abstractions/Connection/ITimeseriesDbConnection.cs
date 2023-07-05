using Npgsql;

namespace Mess.Timeseries.Abstractions.Connection;

public interface ITimeseriesDbConnection
{
  public NpgsqlConnection Value { get; }
}
