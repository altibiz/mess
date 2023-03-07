namespace Mess.System.Extensions.Object;

public static class ObjectSetFieldOrPropertyValueExtensions
{
  public static T? SetFieldOrPropertyValue<O, T>(
    this O @this,
    string fieldOrPropertyName,
    T value
  )
  {
    if (@this is null)
    {
      throw new ArgumentNullException($"Argument '{nameof(@this)}' is null");
    }

    var type = @this.GetType();
    return SetFieldValue<O, T>(@this, type, fieldOrPropertyName, value);
  }

  private static T? SetFieldValue<O, T>(
    this O @this,
    global::System.Type type,
    string fieldOrPropertyName,
    T value
  )
  {
    var field = type.GetField(fieldOrPropertyName);
    if (field is null || field.FieldType != typeof(T))
    {
      return SetPropertyValue<O, T>(@this, type, fieldOrPropertyName, value);
    }

    var previous = field.GetValue(@this);
    field.SetValue(@this, value);

    if (previous is null)
    {
      return default(T?);
    }
    return (T)previous;
  }

  private static T? SetPropertyValue<O, T>(
    this O @this,
    global::System.Type type,
    string fieldOrPropertyName,
    T value
  )
  {
    var property = type.GetProperty(fieldOrPropertyName);
    if (property is null)
    {
      return default(T?);
    }

    var previous = property.GetValue(@this);
    property.SetValue(@this, value);

    if (previous is null)
    {
      return default(T?);
    }
    return (T)previous;
  }
}
