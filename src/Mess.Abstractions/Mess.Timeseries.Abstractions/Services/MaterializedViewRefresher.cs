using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;

namespace Mess.Timeseries.Abstractions.Services;

public abstract class MaterializedViewRefresher<T> : IMaterializedViewRefresher
  where T : TimeseriesDbContext
{
  protected abstract string[] Views { get; }

  public void Refresh(IServiceProvider serviceProvider)
  {
    var context = serviceProvider.GetRequiredService<T>();
    var logger = serviceProvider.GetRequiredService<ILogger<MaterializedViewRefresher<T>>>();

    foreach (var view in Views)
    {
      try
      {
        context.Database.ExecuteSqlRaw(
          $"refresh materialized view concurrently {view}"
        );
      }
      catch
      {
        logger.LogError(
          "Error occured while refreshing materialized view {}",
          view
        );
      }
    }
  }

  public async Task RefreshAsync(IServiceProvider serviceProvider)
  {
    var context = serviceProvider.GetRequiredService<T>();
    var logger = serviceProvider.GetRequiredService<ILogger<MaterializedViewRefresher<T>>>();

    foreach (var view in Views)
    {
      try
      {
        await context.Database.ExecuteSqlRawAsync(
          $"refresh materialized view concurrently {view}"
        );
      }
      catch
      {
        logger.LogError(
          "Error occured while refreshing materialized view {}",
          view
        );
      }
    }
  }
}
