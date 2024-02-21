using Mess.Timeseries.Abstractions.Context;
using Microsoft.EntityFrameworkCore;

namespace Mess.Timeseries.Abstractions.Services;

public record ViewDescriptor(
  string Name,
  bool IsContinuousAggregate
);

public abstract class MaterializedViewRefresher<T> : IMaterializedViewRefresher
  where T : TimeseriesDbContext
{
  protected abstract IEnumerable<ViewDescriptor> Views { get; }

  public void Refresh(IServiceProvider serviceProvider)
  {
    var context = serviceProvider.GetRequiredService<T>();
    var logger = serviceProvider.GetRequiredService<ILogger<MaterializedViewRefresher<T>>>();

    var now = DateTimeOffset.UtcNow;
    var endDate = new DateTimeOffset(
      now.Year,
      now.Month,
      now.Day,
      now.Hour,
      0,
      0,
      now.Offset
    );
    var startDate = endDate.AddHours(-1);

    foreach (var view in Views)
    {
      try
      {
        if (view.IsContinuousAggregate)
        {
          context.Database.ExecuteSqlRaw(
            $@"call refresh_continuous_aggregate(
              '""{view.Name}""',
              '{startDate:o}',
              '{endDate:o}'
            );"
          );
        }
        else
        {
          context.Database.ExecuteSqlRaw(
            $"refresh materialized view concurrently \"{view.Name}\";"
          );
        }
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

    var now = DateTimeOffset.UtcNow;
    var endDate = new DateTimeOffset(
      now.Year,
      now.Month,
      now.Day,
      now.Hour,
      0,
      0,
      now.Offset
    );
    var startDate = endDate.AddHours(-1);

    foreach (var view in Views)
    {
      try
      {
        if (view.IsContinuousAggregate)
        {
          await context.Database.ExecuteSqlRawAsync(
            $@"call refresh_continuous_aggregate(
              '""{view.Name}""',
              '{startDate:o}',
              '{endDate:o}'
            );"
          );
        }
        else
        {
          await context.Database.ExecuteSqlRawAsync(
            $"refresh materialized view concurrently \"{view.Name}\";"
          );
        }
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
