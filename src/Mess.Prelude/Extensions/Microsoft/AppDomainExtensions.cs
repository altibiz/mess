using System.Collections.Concurrent;

namespace Mess.Prelude.Extensions.Microsoft;

public static class AppDomainExtensions
{
  private static readonly ConcurrentDictionary<string, Type?> _typeByNameCache =
    new();

  private static readonly ConcurrentDictionary<
    Type,
    IReadOnlyList<Type>
  > _typesAssignableToCache =
    new();

  public static Type? FindTypeByName(this AppDomain domain, string name)
  {
    return _typeByNameCache.GetOrAdd(
      name,
      name =>
        domain
          .GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .FirstOrDefault(type => type.FullName == name)
    );
  }

  public static IReadOnlyList<Type> FindTypesAssignableTo<T>(
    this AppDomain domain
  )
  {
    return _typesAssignableToCache.GetOrAdd(
      typeof(T),
      type =>
        domain
          .GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .Where(type => type.IsAssignableTo(typeof(T)))
          .ToList()
    );
  }
}
