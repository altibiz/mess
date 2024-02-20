using Mess.Timeseries.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;
using OrchardCore.Environment.Shell;

namespace Mess.Timeseries;

[BackgroundTask(Schedule = "0 */1 * * *")] // NOTE: first minute of every hour
public class MaterializedViewRefreshBackgroundTask : IBackgroundTask
{
  public async Task DoWorkAsync(
    IServiceProvider serviceProvider,
    CancellationToken cancellationToken
  )
  {
    var shellSettings = serviceProvider.GetRequiredService<ShellSettings>();

    if (!shellSettings.IsDefaultShell())
    {
      return;
    }

    var refreshers = serviceProvider.GetServices<IMaterializedViewRefresher>();
    var logger = serviceProvider.GetRequiredService<ILogger<MaterializedViewRefreshBackgroundTask>>();

    foreach (var refresher in refreshers)
    {
      try
      {
        await refresher.RefreshAsync(serviceProvider);
      }
      catch (Exception exception)
      {
        logger.LogError("An error happened whil refreshing materialized views with {}: {}", refresher.GetType().FullName, exception);
      }
    }
  }
}
