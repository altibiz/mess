namespace Mess.Timeseries.Abstractions.Services;

public interface IMaterializedViewRefresher
{
  public void Refresh(IServiceProvider serviceProvider);

  public Task RefreshAsync(IServiceProvider serviceProvider);
}
