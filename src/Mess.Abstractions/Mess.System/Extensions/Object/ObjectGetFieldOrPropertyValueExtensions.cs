namespace Mess.System.Extensions.Object;

public static class ObjectGetFieldOrPropertyValueExtensions
{
  public static object? GetFieldOrPropertyValue<O>(
    this O @this,
    string fieldOrPropertyName
  ) => GetFieldOrPropertyValue<O, object>(@this, fieldOrPropertyName);

  public static T? GetFieldOrPropertyValue<O, T>(
    this O @this,
    string fieldOrPropertyName
  )
  {
    if (@this is null)
    {
      throw new ArgumentNullException($"Argument '{nameof(@this)}' is null");
    }

    var type = @this.GetType();
    return GetFieldValue<O, T>(@this, type, fieldOrPropertyName);
  }

  private static T? GetFieldValue<O, T>(
    this O @this,
    global::System.Type type,
    string fieldOrPropertyName
  )
  {
    var field = type.GetField(fieldOrPropertyName);
    if (field is null || field.FieldType != typeof(T))
    {
      return GetPropertyValue<O, T>(@this, type, fieldOrPropertyName);
    }

    var @value = field.GetValue(@this);
    if (@value is null)
    {
      return default(T?);
    }

    return (T)@value;
  }

  private static T? GetPropertyValue<O, T>(
    this O @this,
    global::System.Type type,
    string fieldOrPropertyName
  )
  {
    var property = type.GetProperty(fieldOrPropertyName);
    if (property is null || property.PropertyType != typeof(T))
    {
      return default(T?);
    }

    var @value = property.GetValue(@this);
    if (@value is null)
    {
      return default(T?);
    }

    return (T)@value;
  }
}
