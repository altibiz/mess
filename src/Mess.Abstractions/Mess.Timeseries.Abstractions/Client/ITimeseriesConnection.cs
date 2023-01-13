using Npgsql;

namespace Mess.Timeseries.Abstractions.Client;

public interface ITimeseriesConnection
{
  public NpgsqlConnection Value { get; }
}
