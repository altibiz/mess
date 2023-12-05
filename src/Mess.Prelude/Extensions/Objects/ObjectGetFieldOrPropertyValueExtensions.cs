namespace Mess.Prelude.Extensions.Objects;

public static class ObjectGetFieldOrPropertyValueExtensions
{
  public static object? GetFieldOrPropertyValue<TObject>(
    this TObject @this,
    string fieldOrPropertyName
  )
  {
    return GetFieldOrPropertyValue<TObject, object>(@this, fieldOrPropertyName);
  }

  public static TValue? GetFieldOrPropertyValue<TObject, TValue>(
    this TObject @this,
    string fieldOrPropertyName
  )
  {
    if (@this is null) throw new ArgumentNullException(nameof(@this));

    var type = @this.GetType();
    return GetFieldValue<TObject, TValue>(@this, type, fieldOrPropertyName);
  }

  private static TValue? GetFieldValue<TObject, TValue>(
    this TObject @this,
    Type type,
    string fieldOrPropertyName
  )
  {
    var field = type.GetField(fieldOrPropertyName);
    if (field is null || field.FieldType != typeof(TValue))
      return GetPropertyValue<TObject, TValue>(@this, type,
        fieldOrPropertyName);

    var value = field.GetValue(@this);

    return value is null ? default : (TValue)value;
  }

  private static TValue? GetPropertyValue<TObject, TValue>(
    this TObject @this,
    Type type,
    string fieldOrPropertyName
  )
  {
    var property = type.GetProperty(fieldOrPropertyName);
    if (property is null) return default;

    var value = property.GetValue(@this);

    return value is null ? default : (TValue)value;
  }
}
