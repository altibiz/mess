namespace Mess.Prelude.Extensions.Microsoft;

public static class IServiceProviderExtensions
{
  public static IEnumerable<T> GetServicesInheriting<T>(
    this IServiceProvider services
  )
    where T : class
  {
    return AppDomain.CurrentDomain
      .GetAssemblies()
      .Where(assembly => assembly.GetName().Name?.StartsWith("Mess") ?? false)
      .SelectMany(assembly => assembly.GetTypes())
      .Where(type => type.IsAssignableTo(typeof(T)))
      .Select(services.GetService)
      .Where(service => service is not null)
      .Cast<T>();
  }
}
