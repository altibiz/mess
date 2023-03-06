namespace Mess.System.Extensions.Type;

public static class TypeGetFieldAndPropertyNamesExtensions
{
  public static IReadOnlyList<string> GetFieldAndPropertyNames(
    this global::System.Type type
  )
  {
    return type.GetFields()
      .Select(field => field.Name)
      .Concat(type.GetProperties().Select(property => property.Name))
      .ToList();
  }
}
