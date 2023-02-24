using System.Collections.Concurrent;

namespace Mess.EventStore.Extensions.Microsoft;

public static class AppDomainExtensions
{
  public static Type? FindTypeByName(this AppDomain domain, string name) =>
    _typeCache.GetOrAdd(
      name,
      name =>
        domain
          .GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .FirstOrDefault(type => type.FullName == name)
    );

  private static ConcurrentDictionary<string, Type?> _typeCache =
    new ConcurrentDictionary<string, Type?>();
}
