using System.Reflection;

namespace Mess.Timeseries.Extensions.Microsoft;

public static class IServiceProviderExtensions
{
  public static IEnumerable<T> GetServicesInheriting<T>(
    this IServiceProvider services
  ) where T : class =>
    Assembly
      .GetCallingAssembly()
      .GetTypes()
      .Where(type => type.IsAssignableTo(typeof(T)))
      .Select(type => services.GetService(type))
      .Where(service => service is not null)
      .Cast<T>();
}
