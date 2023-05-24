using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Abstractions.Extensions.Microsoft;

public static class TimeseriesContextIServiceProviderExtensions
{
  public static async Task<T> WithTimeseriesContextAsync<C, T>(
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

  public static T WithTimeseriesContext<C, T>(
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

  public static async Task WithTimeseriesContextAsync<C>(
    this IServiceProvider services,
    Func<C, Task> todo
  )
    where C : TimeseriesDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    await todo(context);
  }

  public static void WithTimeseriesContext<C>(
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
