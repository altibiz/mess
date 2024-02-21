using Mess.Relational.Abstractions.Context;

namespace Mess.Relational.Abstractions.Extensions;

// TODO: scoped/singleton detection

public static class IServiceProviderExtensions
{
  public static async Task<TReturn> WithRelationalDbContextAsync<
    TContext,
    TReturn
  >(this IServiceProvider services, Func<TContext, Task<TReturn>> todo)
    where TContext : RelationalDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    var result = await todo(context);
    return result;
  }

  public static TReturn WithRelationalDbContext<TContext, TReturn>(
    this IServiceProvider services,
    Func<TContext, TReturn> todo
  )
    where TContext : RelationalDbContext
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    var result = todo(context);
    return result;
  }

  public static async Task WithRelationalDbContextAsync<TContext>(
    this IServiceProvider services,
    Func<TContext, Task> todo
  )
    where TContext : RelationalDbContext
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    await todo(context);
  }

  public static void WithRelationalDbContext<TContext>(
    this IServiceProvider services,
    Action<TContext> todo
  )
    where TContext : RelationalDbContext
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TContext>();
    todo(context);
  }
}
