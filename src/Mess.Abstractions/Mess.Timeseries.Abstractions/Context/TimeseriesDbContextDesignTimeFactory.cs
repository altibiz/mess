using Mess.Relational.Abstractions.Context;

namespace Mess.Timeseries.Abstractions.Context;

public abstract class TimeseriesDbContextDesignTimeFactory<T>
  : RelationalDbContextDesignTimeFactory<T>
  where T : TimeseriesDbContext
{ }
