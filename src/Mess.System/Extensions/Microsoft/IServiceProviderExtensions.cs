namespace Mess.System.Extensions.Microsoft;

public static class IServiceProviderExtensions
{
  public static IEnumerable<T> GetServicesInheriting<T>(
    this IServiceProvider services
  )
    where T : class =>
    AppDomain.CurrentDomain
      .GetAssemblies()
      .SelectMany(assembly => assembly.GetTypes())
      .Where(type => type.IsAssignableTo(typeof(T)))
      .Select(services.GetService)
      .Where(service => service is not null)
      .Cast<T>();
}
