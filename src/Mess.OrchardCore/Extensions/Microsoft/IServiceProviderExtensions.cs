namespace Mess.System.Extensions.Microsoft;

public static class IServiceProviderExtensions
{
  public static async Task<T> AwaitScopeAsync<T>(
    this IServiceProvider serviceProvider,
    Func<IServiceProvider, Task<T>> func
  )
  {
    await using var scope = serviceProvider.CreateAsyncScope();
    return await func(scope.ServiceProvider);
  }

  public static async Task AwaitScopeAsync(
    this IServiceProvider serviceProvider,
    Func<IServiceProvider, Task> func
  )
  {
    await using var scope = serviceProvider.CreateAsyncScope();
    await func(scope.ServiceProvider);
  }

  public static async Task<T> ScopeAsync<T>(
    this IServiceProvider serviceProvider,
    Func<IServiceProvider, Task<T>> func
  )
  {
    using var scope = serviceProvider.CreateScope();
    return await func(scope.ServiceProvider);
  }

  public static async Task ScopeAsync(
    this IServiceProvider serviceProvider,
    Func<IServiceProvider, Task> func
  )
  {
    using var scope = serviceProvider.CreateScope();
    await func(scope.ServiceProvider);
  }

  public static async Task<T> AwaitScope<T>(
    this IServiceProvider serviceProvider,
    Func<IServiceProvider, T> func
  )
  {
    await using var scope = serviceProvider.CreateAsyncScope();
    return func(scope.ServiceProvider);
  }

  public static async Task AwaitScope(
    this IServiceProvider serviceProvider,
    Action<IServiceProvider> func
  )
  {
    await using var scope = serviceProvider.CreateAsyncScope();
    func(scope.ServiceProvider);
  }

  public static T Scope<T>(
    this IServiceProvider serviceProvider,
    Func<IServiceProvider, T> func
  )
  {
    using var scope = serviceProvider.CreateScope();
    return func(scope.ServiceProvider);
  }

  public static void Scope(
    this IServiceProvider serviceProvider,
    Action<IServiceProvider> func
  )
  {
    using var scope = serviceProvider.CreateScope();
    func(scope.ServiceProvider);
  }
}
