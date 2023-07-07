using System.Collections.Concurrent;

namespace Mess.System.Extensions.Microsoft;

public static class AppDomainExtensions
{
  public static Type? FindTypeByName(this AppDomain domain, string name) =>
    _typeByNameCache.GetOrAdd(
      name,
      name =>
        domain
          .GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .FirstOrDefault(type => type.FullName == name)
    );

  private static ConcurrentDictionary<string, Type?> _typeByNameCache =
    new ConcurrentDictionary<string, Type?>();

  public static IReadOnlyList<Type> FindTypesAssignableTo<T>(
    this AppDomain domain
  ) =>
    _typesAssignableToCache.GetOrAdd(
      typeof(T),
      type =>
        domain
          .GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .Where(type => type.IsAssignableTo(typeof(T)))
          .ToList()
    );

  private static ConcurrentDictionary<
    Type,
    IReadOnlyList<Type>
  > _typesAssignableToCache =
    new ConcurrentDictionary<Type, IReadOnlyList<Type>>();
}
