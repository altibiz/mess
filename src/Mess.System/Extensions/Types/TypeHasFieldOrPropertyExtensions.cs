using System.Reflection;

namespace Mess.System.Extensions.Types;

public static class TypeHasFieldOrPropertyValueExtensions
{
  public static bool HasFieldOrProperty<T>(
    this Type @this,
    string fieldOrPropertyName
  )
  {
    return @this is null
      ? throw new ArgumentNullException(nameof(@this))
      : @this.GetField(fieldOrPropertyName) switch
    {
      FieldInfo field => field.FieldType == typeof(T),
      _
        => @this.GetProperty(fieldOrPropertyName) switch
        {
          PropertyInfo property => property.PropertyType == typeof(T),
          _ => false
        }
    };
  }
}
