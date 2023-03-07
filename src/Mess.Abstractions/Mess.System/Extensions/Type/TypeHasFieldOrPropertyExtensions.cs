using System.Reflection;

namespace Mess.System.Extensions.Type;

public static class TypeHasFieldOrPropertyValueExtensions
{
  public static bool HasFieldOrProperty<T>(
    this global::System.Type @this,
    string fieldOrPropertyName
  )
  {
    if (@this is null)
    {
      throw new ArgumentNullException($"Argument '{nameof(@this)}' is null");
    }

    return @this.GetField(fieldOrPropertyName) switch
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
