using Mess.Timeseries.Abstractions.Context;

namespace Mess.Timeseries.Abstractions.Extensions;

public static class TimeseriesDbContextIServiceProviderExtensions
{
  public static async Task<TReturn> WithTimeseriesDbContextAsync<
    TContext,
    TReturn
  >(this IServiceProvider services, Func<TContext, Task<TReturn>> todo)
    where TContext : TimeseriesDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    var result = await todo(context);
    return result;
  }

  public static TReturn WithTimeseriesDbContext<TContext, TReturn>(
    this IServiceProvider services,
    Func<TContext, TReturn> todo
  )
    where TContext : TimeseriesDbContext
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    var result = todo(context);
    return result;
  }

  public static async Task WithTimeseriesDbContextAsync<TContext>(
    this IServiceProvider services,
    Func<TContext, Task> todo
  )
    where TContext : TimeseriesDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    await todo(context);
  }

  public static void WithTimeseriesDbContext<TContext>(
    this IServiceProvider services,
    Action<TContext> todo
  )
    where TContext : TimeseriesDbContext
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    todo(context);
  }
}
