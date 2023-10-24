using Mess.Relational.Abstractions.Context;

namespace Mess.Relational.Abstractions.Extensions;

public static class RelationalDbContextIServiceProviderExtensions
{
  public static async Task<T> WithRelationalDbContextAsync<C, T>(
    this IServiceProvider services,
    Func<C, Task<T>> todo
  )
    where C : RelationalDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    var result = await todo(context);
    return result;
  }

  public static T WithRelationalDbContext<C, T>(
    this IServiceProvider services,
    Func<C, T> todo
  )
    where C : RelationalDbContext
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    var result = todo(context);
    return result;
  }

  public static async Task WithRelationalDbContextAsync<C>(
    this IServiceProvider services,
    Func<C, Task> todo
  )
    where C : RelationalDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    await todo(context);
  }

  public static void WithRelationalDbContext<C>(
    this IServiceProvider services,
    Action<C> todo
  )
    where C : RelationalDbContext
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<C>();
    todo(context);
  }
}
