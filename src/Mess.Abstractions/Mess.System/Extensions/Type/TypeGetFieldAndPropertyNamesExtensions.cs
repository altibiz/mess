namespace Mess.System.Extensions.Type;

public static class TypeGetFieldAndPropertyNamesExtensions
{
  public static IReadOnlyList<string> GetFieldAndPropertyNames<T>(
    this global::System.Type type
  )
  {
    return type.GetFields()
      .Where(field => field.FieldType == typeof(T))
      .Select(field => field.Name)
      .Concat(
        type.GetProperties()
          .Where(property => property.PropertyType == typeof(T))
          .Select(property => property.Name)
      )
      .ToList();
  }
}
