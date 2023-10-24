using Mess.Timeseries.Abstractions.Context;

namespace Mess.Timeseries.Abstractions.Extensions;

public static class TimeseriesDbContextIServiceProviderExtensions
{
  public static async Task<T> WithTimeseriesDbContextAsync<C, T>(
    this IServiceProvider services,
    Func<C, Task<T>> todo
  )
    where C : TimeseriesDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    var result = await todo(context);
    return result;
  }

  public static T WithTimeseriesDbContext<C, T>(
    this IServiceProvider services,
    Func<C, T> todo
  )
    where C : TimeseriesDbContext
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    var result = todo(context);
    return result;
  }

  public static async Task WithTimeseriesDbContextAsync<C>(
    this IServiceProvider services,
    Func<C, Task> todo
  )
    where C : TimeseriesDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    await todo(context);
  }

  public static void WithTimeseriesDbContext<C>(
    this IServiceProvider services,
    Action<C> todo
  )
    where C : TimeseriesDbContext
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    todo(context);
  }
}
